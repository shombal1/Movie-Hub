namespace MovieHub.Engine.Domain.UseCases.AddMedia.InitiateMovieAddition;

public interface IInitiateMovieAdditionStorage : IStorage
{
    public Task<Guid> CreateMovieRequest(
        string title,
        string description,
        DateOnly releasedAt,
        DateTimeOffset publishedAt,
        IEnumerable<string> countries,
        IEnumerable<string> genres,
        IEnumerable<string> directors,
        IEnumerable<string> actors,
        string ageRating,
        long? budget,
        CancellationToken cancellationToken);
}