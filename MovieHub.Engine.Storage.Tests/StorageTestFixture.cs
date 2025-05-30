using System.Reflection;
using Amazon.Runtime;
using Amazon.S3;
using AutoMapper;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MovieHub.Engine.Storage.Mapping;
using StackExchange.Redis;
using Testcontainers.Minio;
using Testcontainers.MongoDb;
using Testcontainers.Redis;

namespace MovieHub.Engine.Storage.Tests;

public class StorageTestFixture : IAsyncLifetime
{
    private readonly MongoDbContainer _mongoDbContainer = new MongoDbBuilder()
        .WithReplicaSet()
        .Build();

    private readonly RedisContainer _redisContainer = new RedisBuilder().Build();

    private readonly MinioContainer _minioContainer = new MinioBuilder()
        .WithUsername("minioadmin")
        .WithPassword("minioadmin")
        .Build();

    private Lazy<Task> _lazyRedisContainerStart;
    private Lazy<Task> _lazyMinioContainerStart;

    public MovieHubDbContext GetMovieHubDbContext()
    {
        return new MovieHubDbContext(new OptionsWrapper<MongoDbConfigure>(new MongoDbConfigure()
        {
            NameDataBase = "test",
            NameMediaCollection = "Media",
            NameMediaBasketCollection = "MediaBasket",
            NameUserCollection = "Users",
            NameSeasonCollection = "Seasons",
            NameAdditionMediaInfoCollection = "AdditionMediaInfo",
            NameDomainEventCollection = "DomainEvents",
            NameMovieRequestCollection = "MovieRequests",
        }), new MongoClient(_mongoDbContainer.GetConnectionString()));
    }

    public IDatabase GetRedisDataBase()
    {
        _lazyRedisContainerStart.Value.GetAwaiter().GetResult();
        return ConnectionMultiplexer.Connect(_redisContainer.GetConnectionString()).GetDatabase(1);
    }

    public async Task<IAmazonS3> GetAmazonS3()
    {
        await _lazyMinioContainerStart.Value;

        var config = new AmazonS3Config
        {
            ServiceURL = _minioContainer.GetConnectionString(),
            ForcePathStyle = true,
            UseHttp = true,
        };

        var s3Client = new AmazonS3Client(
            new BasicAWSCredentials(
                _minioContainer.GetAccessKey(),
                _minioContainer.GetSecretKey()),
            config);

        return s3Client;
    }

    public IMapper GetMapper()
    {
        return new Mapper(
            new MapperConfiguration(v => v.AddMaps(Assembly.GetAssembly(typeof(MediaProfile)))));
    }

    static StorageTestFixture()
    {
        GlobalMongoSetting.Configure();
    }

    public virtual async Task InitializeAsync()
    {
        await _mongoDbContainer.StartAsync();

        _lazyRedisContainerStart =
            new Lazy<Task>(() => _redisContainer.StartAsync(), LazyThreadSafetyMode.ExecutionAndPublication);
        _lazyMinioContainerStart =
            new Lazy<Task>(() => _minioContainer.StartAsync(), LazyThreadSafetyMode.ExecutionAndPublication);

        string initializationScript =
            """
            db = db.getSiblingDB('test');

            db.createCollection("Users");
            db.Users.createIndex({_id: 1});

            db.createCollection("Media");
            db.Media.createIndex({_id: 1});
            db.Media.createIndex({releasedYearAt: 1});
            db.Media.createIndex({genres: 1}); 
            db.Media.createIndex({countries: 1}); 

            db.createCollection("MediaBasket");
            db.MediaBasket.createIndex({userId: 1});
            db.MediaBasket.createIndex({mediaId: 1});
            db.MediaBasket.createIndex({userId: 1, mediaId: 1}, { unique: true });

            db.createCollection("Seasons");
            db.Seasons.createIndex({seriesId: 1});

            db.createCollection("AdditionMediaInfo");
            db.AdditionMediaInfo.createIndex({mediaId: 1});

            db.createCollection("DomainEvents");
            db.DomainEvents.createIndex({_id: 1});

            db.createCollection("MovieRequests");
            db.MovieRequests.createIndex({_id: 1});
            """;

        await _mongoDbContainer.ExecScriptAsync(initializationScript);
    }

    public virtual async Task DisposeAsync()
    {
        await _mongoDbContainer.DisposeAsync();
        await _redisContainer.DisposeAsync();
        await _minioContainer.DisposeAsync();
    }
}