using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using MovieHub.MediaCompressor.Domain;

namespace MovieHub.MediaCompressor;

public class MediaCompressorConsumer(
    IConsumer<byte[], byte[]> consumer,
    ILogger<MediaCompressorConsumer> logger,
    IOptions<ConsumerConfig> options,
    IOptions<KafkaTopic> kafkaTopicOptions,
    IServiceScopeFactory serviceScope) : BackgroundService
{
    private readonly ConsumerConfig _consumerConfig = options.Value;
    private readonly string _topicName = kafkaTopicOptions.Value.Name;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            logger.LogInformation("Starting {TopicName}", _topicName);
            await Task.Yield();

            consumer.Subscribe(_topicName);

            while (!stoppingToken.IsCancellationRequested)
            {
                logger.LogInformation("Waiting for {TopicName}", _topicName);
                var consumerResult = consumer.Consume(stoppingToken);
                
                if (consumerResult.IsPartitionEOF)
                {
                    await Task.Delay(300, stoppingToken);
                    continue;
                }
                
                logger.LogInformation("Consuming {TopicName} offset {Offset}", _topicName,consumerResult.Offset);
                S3NotificationMessage eventEntity =
                    JsonSerializer.Deserialize<S3NotificationMessage>(consumerResult.Message.Value)!;

                int numberRecord = 0;
                
                logger.LogInformation("Processing {TopicName}", _topicName);
                foreach (var record in eventEntity.Records)
                {
                    string key = eventEntity.Key;
                    key = key.Replace(record.S3.Bucket.Name + "/", "");
                    
                    logger.LogInformation("start record {NumberRecord}", numberRecord);
                    
                    if (record.EventName != "s3:ObjectCreated:CompleteMultipartUpload")
                        continue;

                    // TODO add handle series
                    if (record.S3.Object.UserMetadata["X-Amz-Meta-Type"] != "movie")
                        continue;

                    await using (var scope = serviceScope.CreateAsyncScope())
                    {
                        var serviceProvider = scope.ServiceProvider;

                        var movieCompressUseCasse = serviceProvider.GetRequiredService<IMovieCompressUseCase>();

                        await movieCompressUseCasse.Compress(key, record, stoppingToken);
                    }

                    logger.LogInformation("complete record {NumberRecord}", numberRecord);
                }

                consumer.Commit(consumerResult);
            }
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error occurred during consumption");
        }
    }
}