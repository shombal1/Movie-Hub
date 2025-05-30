using FluentValidation;
using MediatR;
using MovieHub.Engine.Domain.DomainEvents;

namespace MovieHub.Engine.Domain.UseCases.AddMedia.InitiateMovieAddition;

public class InitiateMovieAdditionUseCase(
    IValidator<InitiateMovieAdditionCommand> validator,
    IUnitOfWork unitOfWork) 
    : IRequestHandler<InitiateMovieAdditionCommand, Guid>
{
    public async Task<Guid> Handle(InitiateMovieAdditionCommand addition, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(addition, cancellationToken: cancellationToken);

        var scope = await unitOfWork.StartScope(ReadPreference.Primary, cancellationToken);

        var domainEventStorage = scope.GetStorage<IDomainEventStorage>();
        var createMovieRequestStorage = scope.GetStorage<IInitiateMovieAdditionStorage>();

        var movieId = await createMovieRequestStorage.CreateMovieRequest(
            addition.Title,
            addition.Description,
            addition.ReleasedAt,
            addition.PublishedAt,
            addition.Countries,
            addition.Genres,
            addition.Directors,
            addition.Actors,
            addition.AgeRating,
            addition.Budget,
            cancellationToken);

        await domainEventStorage.AddEvent(new DomainEventInitiateMovieAddition(movieId), cancellationToken);

        await scope.Commit(cancellationToken);

        return movieId;
    }
}