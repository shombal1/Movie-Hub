using FluentValidation;
using FluentValidation.Results;
using MediatR;
using MovieHub.Engine.Domain.DomainEvents;
using MovieHub.Engine.Domain.Exceptions;
using MovieHub.Engine.Domain.UseCases.AddMedia.StartMovieUpload;

namespace MovieHub.Engine.Domain.UseCases.AddMedia.FinalizeMovieAddition;

public class FinalizeMovieAdditionUseCase(
    IGetMovieRequestStorage getMovieRequestStorage,
    IValidator<FinalizeMovieAdditionCommand> validator, 
    IUnitOfWork unitOfWork)
    : IRequestHandler<FinalizeMovieAdditionCommand, Unit>
{
    public async Task<Unit> Handle(FinalizeMovieAdditionCommand request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        var requestId = request.RequestId;

        var movieRequest = await getMovieRequestStorage.GetMovieRequest(requestId, cancellationToken);

        if (movieRequest is null)
            throw new MediaRequestNotFoundException(requestId);

        List<ValidationFailure> validationFailures = [];

        if (movieRequest.OriginalUrlKey is null)
        {
            validationFailures.Add(new ValidationFailure()
                {
                    PropertyName = nameof(movieRequest.OriginalUrlKey),
                    ErrorCode = "Empty",
                    ErrorMessage = "UrlKey is not set"
                }
            );
        }

        if (validationFailures.Count != 0)
        {
            throw new ValidationException(validationFailures);
        }

        var scope = await unitOfWork.StartScope(ReadPreference.Primary, cancellationToken);
        
        var updateStatusMovieRequest = scope.GetStorage<IFinalizeMovieAdditionStorage>();
        var domainEventStorage = scope.GetStorage<IDomainEventStorage>();
        
        var taskSetStatus = updateStatusMovieRequest.Update(requestId,cancellationToken);
        var taskDomainEvent = domainEventStorage.AddEvent(new DomainEventFinalizeMovieAddition(requestId, movieRequest.OriginalUrlKey!), cancellationToken);

        await Task.WhenAll(taskSetStatus, taskDomainEvent);
        
        await scope.Commit(cancellationToken);
        
        return Unit.Value;
    }
}