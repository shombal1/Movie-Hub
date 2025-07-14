using FluentValidation;
using MediatR;
using MovieHub.Engine.Domain.DomainEvents;
using MovieHub.Engine.Domain.Exceptions;
using MovieHub.Engine.Domain.UseCases.GetPerson;

namespace MovieHub.Engine.Domain.UseCases.AddMedia.InitiateMovieAddition;

public class InitiateMovieAdditionUseCase(
    IValidator<InitiateMovieAdditionCommand> validator,
    IUnitOfWork unitOfWork) 
    : IRequestHandler<InitiateMovieAdditionCommand, Guid>
{
    public async Task<Guid> Handle(InitiateMovieAdditionCommand request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken: cancellationToken);

        var scope = await unitOfWork.StartScope(ReadPreference.Primary, cancellationToken);

        var domainEventStorage = scope.GetStorage<IDomainEventStorage>();
        var createMovieRequestStorage = scope.GetStorage<IInitiateMovieAdditionStorage>();
        var getPersonStorage = scope.GetStorage<IGetPersonStorage>();

        var personIds = request.ActorIds.Concat(request.DirectorIds).ToArray();
        await getPersonStorage.ThrowIfPersonsNotFound(personIds, cancellationToken);
        
        var movieId = await createMovieRequestStorage.CreateMovieRequest(
            request.Title,
            request.Description,
            request.ReleasedAt,
            request.Countries,
            request.Genres,
            request.DirectorIds,
            request.ActorIds,
            request.AgeRating,
            request.Budget,
            cancellationToken);

        await domainEventStorage.AddEvent(new DomainEventInitiateMovieAddition(movieId), cancellationToken);

        await scope.Commit(cancellationToken);

        return movieId;
    }
}