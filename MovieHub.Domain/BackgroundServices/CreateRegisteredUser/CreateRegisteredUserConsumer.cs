using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using MovieHub.Domain.UseCases;

namespace MovieHub.Domain.BackgroundServices.CreateRegisteredUser;

public class CreateRegisteredUserConsumer(
    IConsumer<byte[], byte[]> consumer,
    ICreateSynchronizationUserStorage createSynchronizationUserStorage) : BackgroundService
{
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

            EventEntity eventEntity = JsonSerializer.Deserialize<EventEntity>(consumerResult.Message.Value)!;

            try
            {
                if (eventEntity is { Type: "REGISTER", UserId: not null })
                {
                    await createSynchronizationUserStorage.Create(eventEntity.UserId.Value, stoppingToken);
                }
            }
            catch (Exception)
            {
                // ignored
            }

            consumer.Commit(consumerResult);
        }
        
        consumer.Close();
    }
}