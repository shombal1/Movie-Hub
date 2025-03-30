using System.Reflection;
using AutoMapper;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using MovieHub.Engine.Storage.Mapping;
using StackExchange.Redis;
using Testcontainers.MongoDb;
using Testcontainers.Redis;

namespace MovieHub.Engine.Storage.Tests;

public class StorageTestFixture : IAsyncLifetime
{
    private readonly MongoDbContainer _mongoDbContainer = new MongoDbBuilder()
        .WithUsername("")
        .WithPassword("")
        .Build();

    private readonly RedisContainer _redisContainer = new RedisBuilder().Build();

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
        }), new MongoClient( _mongoDbContainer.GetConnectionString()));
    }

    public IDatabase GetRedisDataBase()
    {
        return ConnectionMultiplexer.Connect(_redisContainer.GetConnectionString()).GetDatabase(1);
    }

    public IMapper GetMapper()
    {
        return new Mapper(
            new MapperConfiguration(v => v.AddMaps(Assembly.GetAssembly(typeof(MediaProfile)))));
    }

    static StorageTestFixture()
    {
        BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
    }
    
    public virtual async Task InitializeAsync()
    { 
        Task.WaitAll(
         _mongoDbContainer.StartAsync(),
            _redisContainer.StartAsync());

        string initializationScript =
            """
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
            """;

        await _mongoDbContainer.ExecScriptAsync(initializationScript);
    }

    public virtual async Task DisposeAsync()
    {
        await _mongoDbContainer.DisposeAsync();
        await _redisContainer.DisposeAsync();
    }
}