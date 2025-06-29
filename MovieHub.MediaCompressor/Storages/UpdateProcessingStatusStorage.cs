using MongoDB.Driver;
using MovieHub.MediaCompressor.Domain;
using MovieHub.MediaCompressor.Mongo;

namespace MovieHub.MediaCompressor.Storages;

public class UpdateProcessingStatusStorage(MovieHubMongoDb dbContext) : IUpdateProcessingStatusStorage
{
    public Task Update(Guid movieId, Dictionary<QualityType, string> processedQualities,
        CancellationToken cancellationToken)
    {
        var filter = Builders<MovieRequestEntity>.Filter.Where(x => x.Id == movieId);
        var update = Builders<MovieRequestEntity>.Update
            .Set(x => x.Status.ProcessedQualities, processedQualities)
            .Set(x => x.Status.IsQualitiesProcessed, true);


        return dbContext.MovieRequests
            .FindOneAndUpdateAsync(dbContext.CurrentSession, filter, update, cancellationToken: cancellationToken);
    }
}