using MediatR;

namespace MovieHub.Engine.Domain.UseCases.AddMedia.InitiateMovieAddition;

public record InitiateMovieAdditionCommand(
    string Title,
    string Description,
    DateOnly ReleasedAt,
    IEnumerable<string> Countries,
    IEnumerable<string> Genres,
    IEnumerable<Guid> DirectorIds,
    IEnumerable<Guid> ActorIds,
    string AgeRating,
    long? Budget) : IRequest<Guid>;