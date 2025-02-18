using System.Text;
using MovieHub.Engine.Domain.Jobs.SyncMediaViews;
using StackExchange.Redis;

namespace MovieHub.Engine.Storage.Storages;

public class PopMediaViewsStorage(IDatabase database) : IPopMediaViewsStorage
{
    public const string KeyFormat = "mediaId:{0}";
    public async Task<long> PopViews(Guid mediaId, CancellationToken cancellationToken)
    {
        var views = await database.StringGetSetAsync(string.Format(KeyFormat, mediaId), 0);
        
        return long.Parse(views.HasValue ? views.ToString() : "0");
    }
}