using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Options;
using MovieHub.AI.Narrator.Domain.Jobs;
using MovieHub.AI.Narrator.Storage;
using Quartz;

namespace MovieHub.AI.Narrator.Integration;

public class MediaEventsConsumer(
    IConsumer<byte[], byte[]> consumer,
    ILogger<MediaEventsConsumer> logger,
    IOptions<ConsumerConfig> options,
    IOptions<KafkaTopic> kafkaTopicOptions,
    ISchedulerFactory schedulerFactory,
    IOptions<BackgroundJobOptions> backgroundJobOptions,
    HybridCache hybridCache) : BackgroundService
{
    private readonly ConsumerConfig _consumerConfig = options.Value;
    private readonly string _topicName = kafkaTopicOptions.Value.Name;
    private readonly int _maxConcurrentJobs = backgroundJobOptions.Value.MaxConcurrentJobs;

    public const string JobGroupName = "narrator-group";
    public const string ProcessedMediaPrefix = "processed_media_";

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Yield();
        try
        {
            consumer.Subscribe(_topicName);

            var scheduler = await schedulerFactory.GetScheduler(stoppingToken);
            
            while (!stoppingToken.IsCancellationRequested)
            {
                if (await ShouldWaitForJobSlot(scheduler,stoppingToken))
                {
                    await Task.Delay(3000, stoppingToken);
                    continue;
                }

                ConsumeResult<byte[], byte[]>? consumerResult = ConsumeMessage(stoppingToken);
                if (consumerResult == null)
                {
                    await Task.Delay(300, stoppingToken);
                    continue;
                }

                await ProcessedMessage(consumerResult, scheduler, stoppingToken);
            }
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error occurred during consumption");
        }
        finally
        {
            consumer.Close();
        }
    }

    private async Task ProcessedMessage(ConsumeResult<byte[], byte[]> consumerResult, IScheduler scheduler, CancellationToken stoppingToken)
    {
        logger.LogInformation("Consuming {TopicName} offset {Offset}", _topicName, consumerResult.Offset);
        S3NotificationMessage eventEntity =
            JsonSerializer.Deserialize<S3NotificationMessage>(consumerResult.Message.Value)!;

        int numberRecord = 0;

        logger.LogInformation("Processing {TopicName}", _topicName);
        foreach (var record in eventEntity.Records)
        {
            string key = eventEntity.Key;
            key = key.Replace(record.S3.Bucket.Name + "/", "");

            Guid movieRequestId = Guid.Parse(record.S3.Object.UserMetadata["X-Amz-Meta-Movie-Id"]);

            logger.LogInformation("start record {NumberRecord}", numberRecord);

            if (record.EventName != "s3:ObjectCreated:CompleteMultipartUpload")
                continue;

            // TODO add handle series
            if (record.S3.Object.UserMetadata["X-Amz-Meta-Type"] != "movie")
                continue;

            string mediaKey = $"{ProcessedMediaPrefix}{movieRequestId}_{key}";
            var isProcessed = await hybridCache.GetOrCreateAsync(
                key: mediaKey,
                factory: _ => ValueTask.FromResult(false),
                options: new HybridCacheEntryOptions()
                {
                    Expiration = TimeSpan.FromDays(1)
                },
                cancellationToken: stoppingToken);
            
            if (isProcessed)
            {
                continue;
            }

            Guid jobId = Guid.NewGuid();

            var jobKey = new JobKey($"{JobGroupName}-job-{jobId}", JobGroupName);

            var job = JobBuilder.Create<BackgroundNarratorJob>()
                .WithIdentity(jobKey)
                .UsingJobData(new JobDataMap()
                {
                    ["movieRequestId"] = movieRequestId.ToString(),
                    ["s3Key"] = key
                })
                .StoreDurably()
                .RequestRecovery()
                .Build();

            var trigger = TriggerBuilder.Create()
                .WithIdentity($"{JobGroupName}-trigger-{jobId}", JobGroupName)
                .StartNow()
                .Build();

            await scheduler.ScheduleJob(job, trigger,  stoppingToken);

            logger.LogInformation("complete record {NumberRecord}", numberRecord);
        }

        consumer.Commit(consumerResult);
    }

    private async Task<bool> ShouldWaitForJobSlot(IScheduler scheduler,CancellationToken stoppingToken)
    {
        var runningJobs = await scheduler.GetCurrentlyExecutingJobs(stoppingToken);
        var narratorJobsCount = runningJobs.Count(job => job.JobDetail.Key.Group.StartsWith(JobGroupName));

        if (narratorJobsCount >= _maxConcurrentJobs)
        {
            return true;
        }

        return false;
    }

    private ConsumeResult<byte[], byte[]>? ConsumeMessage(CancellationToken stoppingToken)
    {
        logger.LogInformation("Waiting for {TopicName}", _topicName);
        var result = consumer.Consume(stoppingToken);

        if (result.IsPartitionEOF)
        {
            return null;
        }

        logger.LogInformation("Consuming {TopicName} offset {Offset}", _topicName, result.Offset);
        return result;
    }
}