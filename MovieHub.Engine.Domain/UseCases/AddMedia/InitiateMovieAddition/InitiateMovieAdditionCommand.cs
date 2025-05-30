using MediatR;

namespace MovieHub.Engine.Domain.UseCases.AddMedia.InitiateMovieAddition;

public record InitiateMovieAdditionCommand(
    string Title,
    string Description,
    DateOnly ReleasedAt,
    DateTimeOffset PublishedAt,
    IEnumerable<string> Countries,
    IEnumerable<string> Genres,
    IEnumerable<string> Directors,
    IEnumerable<string> Actors,
    string AgeRating,
    long? Budget) : IRequest<Guid>;