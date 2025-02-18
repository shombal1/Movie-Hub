namespace MovieHub.Engine.Domain.Jobs.SyncMediaViews;

public interface IPushMediaViewsStorage : IStorage
{
    public Task Increment(IEnumerable<(Guid mediaId, long views)> viewCounts,CancellationToken cancellationToken);
}