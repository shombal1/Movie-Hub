namespace MovieHub.Engine.Domain.UseCases.AddMedia.InitiateMovieAddition;

public interface IInitiateMovieAdditionStorage : IStorage
{
    public Task<Guid> CreateMovieRequest(
        string title,
        string description,
        DateOnly releasedAt,
        IEnumerable<string> countries,
        IEnumerable<string> genres,
        IEnumerable<Guid> directorIds,
        IEnumerable<Guid> actorIds,
        string ageRating,
        long? budget,
        CancellationToken cancellationToken);
}