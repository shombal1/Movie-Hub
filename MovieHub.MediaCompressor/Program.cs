using Amazon.S3;
using MongoDB.Driver;
using MovieHub.MediaCompressor;
using MovieHub.MediaCompressor.Domain;
using MovieHub.MediaCompressor.Mongo;
using MovieHub.MediaCompressor.Storages;

var builder = WebApplication.CreateBuilder(args);

ConfigurationManager configuration = builder.Configuration;

GlobalMongoSetting.Configure();


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

builder.Services.AddScoped<IS3StorageService, S3StorageService>();


var app = builder.Build();
app.Run();