using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MovieHub.Engine.Domain.Jobs.SyncMediaViews;

namespace MovieHub.Engine.Storage.Storages;

public class GetMediaIdsStorage(MovieHubDbContext dbContext) : IGetMediaIdsStorage
{
    public async Task<IEnumerable<Guid>> Get(CancellationToken cancellationToken)
    {
        return await dbContext.Media.AsQueryable(dbContext.CurrentSession)
            .Select(m => m.Id)
            .ToListAsync(cancellationToken: cancellationToken);
    }
}