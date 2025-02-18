using MongoDB.Driver;
using MovieHub.Engine.Domain.Jobs.SyncMediaViews;
using MovieHub.Engine.Storage.Entities;

namespace MovieHub.Engine.Storage.Storages;

public class PushMediaViewsStorage(MovieHubDbContext dbContext) : IPushMediaViewsStorage
{
    public async Task Increment(IEnumerable<(Guid mediaId, long views)> viewCounts, CancellationToken cancellationToken)
    {
        var updates = viewCounts.Select(vc =>
            new UpdateOneModel<MediaEntity>(
                Builders<MediaEntity>.Filter.Where(m=>m.Id == vc.mediaId),
                Builders<MediaEntity>.Update.Inc(m=>m.Views, vc.views)
            )
        ).ToList();
        
        if (updates.Any())
        {
            await dbContext.Media.BulkWriteAsync(updates, cancellationToken: cancellationToken);
        }
    }
}