namespace MovieHub.Engine.Domain.Jobs.SyncMediaViews;

public interface IPopMediaViewsStorage
{
    public Task<long> PopViews(Guid mediaId,CancellationToken cancellationToken);
}