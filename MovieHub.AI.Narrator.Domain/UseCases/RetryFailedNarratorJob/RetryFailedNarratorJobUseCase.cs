using FluentValidation;
using FluentValidation.Results;
using MovieHub.AI.Narrator.Domain.Exceptions;
using MovieHub.AI.Narrator.Domain.Jobs;
using MovieHub.AI.Narrator.Domain.Models;
using MovieHub.AI.Narrator.Domain.UseCases.GetFailedNarratorJobs;
using Quartz;

namespace MovieHub.AI.Narrator.Domain.UseCases.RetryFailedNarratorJob;

public class RetryFailedNarratorJobUseCase(
    IValidator<RetryFailedNarratorJobCommand> validator,
    IGetFailedNarratorJobStorage getFailedNarratorJobStorage,
    IFailedNarratorMarkAsRetriedStorage failedNarratorMarkAsRetriedStorage,
    ISchedulerFactory schedulerFactory) 
    : IRetryFailedNarratorJobUseCase
{
    public async Task RetryFailedJob(RetryFailedNarratorJobCommand request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        var oldJobId = request.JobId;
        
        FailedNarratorJob? failedNarratorJob = await getFailedNarratorJobStorage.Get(oldJobId, cancellationToken);

        if (failedNarratorJob is null)
        {
            throw new FailedNarratorJobNotFoundException(oldJobId);
        }

        if (failedNarratorJob.S3Key is null)
        {
            throw new ValidationException([new ValidationFailure(nameof(failedNarratorJob.S3Key), "S3Key is required for retrying failed job")]);
        }
        
        var (job, trigger) = NarratorJobBuilder.CreateJobWithTrigger(failedNarratorJob.MediaId, failedNarratorJob.S3Key);
        var scheduler = await schedulerFactory.GetScheduler(cancellationToken);
        await scheduler.ScheduleJob(job, trigger, cancellationToken);
        
        await failedNarratorMarkAsRetriedStorage.Mark(request.JobId, cancellationToken);
    }
}