using MongoDB.Driver;
using MovieHub.Engine.Domain.UseCases.AddMedia.FinalizeMovieUpload;
using MovieHub.Engine.Storage.Entities;

namespace MovieHub.Engine.Storage.Storages.AddMovie;

public class SetUrlKeyMovieRequestStorage(MovieHubDbContext dbContext) : ISetUrlKeyMovieRequestStorage
{
    public async Task SetUrlKey(Guid requestId, string urlKey, CancellationToken cancellationToken)
    {
        var filter = Builders<MovieRequestEntity>.Filter.Where(x => x.Id == requestId);
        var update = Builders<MovieRequestEntity>.Update
            .Set(x => x.OriginalUrlKey, urlKey);

        await dbContext.MovieRequests.UpdateOneAsync(
            dbContext.CurrentSession,
            filter, 
            update, 
            new UpdateOptions(), 
            cancellationToken);
    }
}