using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MovieHub.MediaCompressor.Domain;
using MovieHub.MediaCompressor.Mongo;

namespace MovieHub.MediaCompressor.Storages;

public class GetProcessingStatusStorage(MovieHubMongoDb dbContext): IGetProcessingStatusStorage
{
    public async Task<ProcessingStatus?> GetFromMovieRequest(Guid movieRequestId,CancellationToken cancellationToken)
    {
        return await dbContext.MovieRequests
            .AsQueryable(dbContext.CurrentSession)
            .Where(x => x.Id == movieRequestId)
            .Select(x => x.Status)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
    }
}