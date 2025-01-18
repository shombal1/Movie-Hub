using System.Diagnostics;
using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MovieHub.UserSync.KeycloakConsumer.Monitoring;

namespace MovieHub.UserSync.KeycloakConsumer;

public class KeycloakEventsConsumer(
    IConsumer<byte[], byte[]> consumer,
    ILogger<KeycloakEventsConsumer> logger,
    ICreateUserStorage createUserStorage,
    IOptions<ConsumerConfig> options) : BackgroundService
{
    private readonly ConsumerConfig _consumerConfig = options.Value;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Yield();

        consumer.Subscribe("movie-hub.keycloak-events");
        
        while (!stoppingToken.IsCancellationRequested)
        {
            var consumerResult = consumer.Consume(stoppingToken);

            if (consumerResult.IsPartitionEOF)
            {
                await Task.Delay(300, stoppingToken);
                continue;
            }

            using Activity? activity = Metrics.ActivitySource.StartActivity("consumer", ActivityKind.Consumer);

            activity?.AddTag("messaging.kafka.group_id", _consumerConfig.GroupId);
            activity?.AddTag("messaging.kafka.partition", consumerResult.Partition.Value);

            EventEntity eventEntity = JsonSerializer.Deserialize<EventEntity>(consumerResult.Message.Value)!;

            try
            {
                if (eventEntity is not { Type: "REGISTER", UserId: not null })
                    continue;

                await createUserStorage.Create(eventEntity.UserId.Value, stoppingToken);
            }
            catch (MongoWriteException e) when (e.WriteError.Category == ServerErrorCategory.DuplicateKey)
            {
                logger.LogWarning(e, "User with id {user_id} already exists", eventEntity.UserId);
            }

            consumer.Commit(consumerResult);
        }

        consumer.Close();
    }
}