using System.Reflection;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using MovieHub.AI.Narrator.Domain.DependencyInjection;
using MovieHub.AI.Narrator.Integration;
using MovieHub.AI.Narrator.Integration.Mapping;
using MovieHub.AI.Narrator.Storage;
using MovieHub.AI.Narrator.Storage.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDomainServices();
builder.Services.AddStorageServices(
    configuration.GetConnectionString("MovieHubDbContext")!,
    configuration.GetConnectionString("S3Storage")!,
    configuration.GetConnectionString("Quartz")!);

builder.Services.Configure<BackgroundJobOptions>(configuration.GetSection("BackgroundJobOptions"));
builder.Services.Configure<GenerateMediaDescriptionOptions>(configuration.GetSection("GenerateMediaDescriptionOptions"));
builder.Services.Configure<DownloadSettings>(configuration.GetSection("DownloadSettings"));
builder.Services.Configure<S3Settings>(configuration.GetSection("S3Settings"));
builder.Services.Configure<MongoDbConfigure>(configuration.GetSection("MongoDbConfigure"));
builder.Services.Configure<KafkaTopic>(configuration.GetSection("KafkaTopic"));

builder.Services.Configure<ConsumerConfig>(configuration
    .GetSection("Kafka").Bind);
builder.Services.Configure<KafkaTopic>(configuration
    .GetSection("KafkaTopic").Bind);
builder.Services.AddSingleton(sp => new ConsumerBuilder<byte[], byte[]>(
    sp.GetRequiredService<IOptions<ConsumerConfig>>().Value).Build());

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = configuration.GetConnectionString("Redis")
                            ?? throw new InvalidOperationException("Redis cache connection string is not configured.");
    options.InstanceName = configuration["Redis:InstanceName"];
});

builder.Services.AddHybridCache();

builder.Services.AddHostedService<MediaEventsConsumer>();

builder.Services.AddAutoMapper(conf => conf.AddMaps(Assembly.GetAssembly(typeof(FailedNarratorJobProfile))));


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var downloadSettings = scope.ServiceProvider.GetRequiredService<IOptions<DownloadSettings>>();
    var localStoragePath = downloadSettings.Value.LocalStoragePath;

    if (string.IsNullOrWhiteSpace(localStoragePath))
    {
        localStoragePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "temp_storage");
    }

    if (!Directory.Exists(localStoragePath))
    {
        Directory.CreateDirectory(localStoragePath);
    }
    
    downloadSettings.Value.LocalStoragePath = localStoragePath;
}

app.Run();