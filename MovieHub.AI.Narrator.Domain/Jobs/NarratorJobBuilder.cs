using Quartz;

namespace MovieHub.AI.Narrator.Domain.Jobs;

public class NarratorJobBuilder
{
    public const string JobGroupName = "narrator-group";

    public static (IJobDetail job, ITrigger trigger) CreateJobWithTrigger(Guid movieRequestId, string s3Key)
    {
        var jobId = Guid.NewGuid();
        var jobKey = new JobKey($"{JobGroupName}-job-{jobId}", JobGroupName);

        var job = JobBuilder.Create<BackgroundNarratorJob>()
            .WithIdentity(jobKey)
            .UsingJobData(new JobDataMap()
            {
                ["movieRequestId"] = movieRequestId.ToString(),
                ["s3Key"] = s3Key
            })
            .StoreDurably()
            .RequestRecovery()
            .Build();

        var trigger = TriggerBuilder.Create()
            .WithIdentity($"{JobGroupName}-trigger-{jobId}", JobGroupName)
            .StartNow()
            .Build();

        return (job, trigger);
    }
}