using Microsoft.EntityFrameworkCore;
using MovieHub.AI.Narrator.Domain.Jobs;

namespace MovieHub.AI.Narrator.Storage.Storages;

public class DeleteFailedNarratorJobStorage(QuartzDbContext dbContext) : IDeleteFailedNarratorJobStorage
{
    public Task Delete(Guid mediaId, CancellationToken cancellationToken)
    {
        return dbContext.FailedNarratorJobs
            .Where(x => x.MediaId == mediaId)
            .ExecuteDeleteAsync(cancellationToken);
    }
}