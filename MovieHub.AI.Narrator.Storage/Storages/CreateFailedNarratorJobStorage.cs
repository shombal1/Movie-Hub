using MovieHub.AI.Narrator.Domain.Jobs;
using MovieHub.AI.Narrator.Storage.QuartzEntities;

namespace MovieHub.AI.Narrator.Storage.Storages;

public class CreateFailedNarratorJobStorage(
    QuartzDbContext dbContext,
    IGuidFactory guidFactory,
    TimeProvider timeProvider) 
    : ICreateFailedNarratorJobStorage
{
    public async Task<Guid> Create(string jobName, string jobGroup, Guid mediaId, string? s3Key, string errorMessage)
    {
        var id = guidFactory.Create();
        var newFailedJob = new FailedNarratorJobEntity
        {
            Id = id,
            JobName = jobName,
            JobGroup = jobGroup,
            MediaId = mediaId,
            S3Key = s3Key,
            ErrorMessage = errorMessage,
            FailedAt = timeProvider.GetUtcNow(),
            IsRetried = false
        };

        dbContext.FailedNarratorJobs.Add(newFailedJob);
        await dbContext.SaveChangesAsync();
        
        return id;
    }
}