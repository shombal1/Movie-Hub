using MovieHub.Engine.Domain.UseCases.IncrementMediaViews;
using StackExchange.Redis;

namespace MovieHub.Engine.Storage.Storages;

public class IncrementMediaViewsStorage(IDatabase database) : IIncrementMediaViewsStorage
{
    public const string KeyFormat = "mediaId:{0}";
    public Task Increment(Guid mediaId,CancellationToken cancellationToken)
    { 
        return database.StringIncrementAsync(string.Format(KeyFormat, mediaId),1);
    }
}