using MovieHub.Engine.Domain.UseCases.AddMedia.InitiateMovieAddition;
using MovieHub.Engine.Storage.Common;
using MovieHub.Engine.Storage.Entities;
using MovieHub.Engine.Storage.Models;

namespace MovieHub.Engine.Storage.Storages.AddMovie;

public class InitiateMovieAdditionStorage(
    MovieHubDbContext dbContext,
    IGuidFactory guidFactory) 
    : IInitiateMovieAdditionStorage
{
    public async Task<Guid> CreateMovieRequest(string title, string description, DateOnly releasedAt, DateTimeOffset publishedAt,
        IEnumerable<string> countries, IEnumerable<string> genres, IEnumerable<string> directors, IEnumerable<string> actors, string ageRating,
        long? budget, CancellationToken cancellationToken)
    {
        var movieRequestId = guidFactory.Create();
        
        var movieRequest = new MovieRequestEntity
        {
            Id = movieRequestId,
            Title = title,
            Description = description,
            ReleasedAt = releasedAt,
            PublishedAt = publishedAt,
            Countries = countries,
            Genres = genres,
            Directors = directors,
            Actors = actors,
            AgeRating = ageRating,
            Budget = budget,
            Status = new ProcessingStatus()
        };
        
        await dbContext.MovieRequests.InsertOneAsync(
            dbContext.CurrentSession,
            movieRequest, 
            cancellationToken: cancellationToken);

        return movieRequestId;
    }
}