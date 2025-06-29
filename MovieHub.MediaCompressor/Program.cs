using Amazon.S3;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MovieHub.MediaCompressor;
using MovieHub.MediaCompressor.Domain;
using MovieHub.MediaCompressor.Mongo;
using MovieHub.MediaCompressor.Storages;

var builder = WebApplication.CreateBuilder(args);

ConfigurationManager configuration = builder.Configuration;

GlobalMongoSetting.Configure();


builder.Services.Configure<ConsumerConfig>(configuration
    .GetSection("Kafka").Bind);
builder.Services.Configure<KafkaTopic>(configuration
    .GetSection("KafkaTopic").Bind);
builder.Services.AddSingleton(sp => new ConsumerBuilder<byte[], byte[]>(
    sp.GetRequiredService<IOptions<ConsumerConfig>>().Value).Build());

builder.Services.Configure<S3Settings>(builder.Configuration.GetSection("S3Settings").Bind);
builder.Services.Configure<MongoDbConfigure>(builder.Configuration.GetSection("MongoDbConfigure").Bind);
builder.Services.Configure<DownloadSettings>(builder.Configuration.GetSection("DownloadSettings").Bind);
builder.Services.AddSingleton<IMongoClient>(_ => new MongoClient(configuration.GetConnectionString("MovieHubMongoDb")));
builder.Services.AddScoped<MovieHubMongoDb>();


string s3StorageConnectionString = configuration.GetConnectionString("S3Storage")!;

builder.Services.AddSingleton<IAmazonS3>(sp =>
{
    var config = new AmazonS3Config
    {
        ServiceURL =
            s3StorageConnectionString.Split(';').First(x => x.StartsWith("Endpoint=")).Replace("Endpoint=", ""),
        ForcePathStyle = true,
        UseHttp = true
    };

    var client = new AmazonS3Client(
        s3StorageConnectionString.Split(';').First(x => x.StartsWith("AccessKey=")).Replace("AccessKey=", ""),
        s3StorageConnectionString.Split(';').First(x => x.StartsWith("SecretKey=")).Replace("SecretKey=", ""),
        config
    );

    return client;
});

builder.Services.AddScoped<IGetProcessingStatusStorage, GetProcessingStatusStorage>();
builder.Services.AddScoped<IMediaQualityAnalyzerStorage, MediaQualityAnalyzerStorage>();
builder.Services.AddScoped<IS3StorageService, S3StorageService>();
builder.Services.AddScoped<IUpdateProcessingStatusStorage, UpdateProcessingStatusStorage>();

builder.Services.AddHostedService<MediaCompressorConsumer>();

builder.Services.AddScoped<IMovieCompressUseCase, MovieCompressUseCase>();

var app = builder.Build();
app.Run();