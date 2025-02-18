namespace MovieHub.Engine.Domain.Jobs.SyncMediaViews;

public interface IGetMediaIdsStorage : IStorage
{
    public Task<IEnumerable<Guid>> Get(CancellationToken cancellationToken);
}