using Microsoft.EntityFrameworkCore;
using MovieHub.AI.Narrator.Domain.UseCases.RetryFailedNarratorJob;

namespace MovieHub.AI.Narrator.Storage.Storages;

public class FailedNarratorMarkAsRetriedStorage(QuartzDbContext dbContext) : IFailedNarratorMarkAsRetriedStorage
{
    public Task Mark(Guid jobId, CancellationToken cancellationToken)
    {
        return dbContext.FailedNarratorJobs
            .Where(x => x.Id == jobId)
            .ExecuteUpdateAsync(x => 
                x.SetProperty(x => x.IsRetried, true), 
                cancellationToken: cancellationToken);
    }
}