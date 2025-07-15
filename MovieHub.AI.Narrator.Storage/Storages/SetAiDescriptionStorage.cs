using MongoDB.Driver;
using MovieHub.AI.Narrator.Domain.UseCases.GenerateMovieDescription;
using MovieHub.AI.Narrator.Storage.Entities;

namespace MovieHub.AI.Narrator.Storage.Storages;

public class SetAiDescriptionStorage(MovieHubDbContext dbContext) : ISetAiDescriptionStorage
{
    public Task Set(Guid movieId, string aiDescription, CancellationToken cancellationToken)
    {
        var filter = Builders<MovieRequestEntity>.Filter.Where(x => x.Id == movieId);
        var update = Builders<MovieRequestEntity>.Update
            .Set(x => x.Status.AiDescription, aiDescription);


        return dbContext.MovieRequests
            .FindOneAndUpdateAsync(dbContext.CurrentSession, filter, update, cancellationToken: cancellationToken);
    }
}