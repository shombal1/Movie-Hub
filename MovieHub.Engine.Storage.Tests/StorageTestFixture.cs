using System.Reflection;
using DotNet.Testcontainers.Builders;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MovieHub.Engine.Storage.Mapping;
using Testcontainers.MongoDb;

namespace MovieHub.Engine.Storage.Tests;

public class StorageTestFixture : IAsyncLifetime
{
    private readonly MongoDbContainer _mongoDbContainer = new MongoDbBuilder()
        .WithUsername("")
        .WithPassword("")
        .WithExtraHost("host.docker.internal", "host-gateway")
        .WithCommand("--replSet", "rs0")
        .WithWaitStrategy(Wait.ForUnixContainer())
        .Build();


    public MovieHubDbContext GetMovieHubDbContext()
    {
        return new MovieHubDbContext(new OptionsWrapper<MongoDbConfigure>(new MongoDbConfigure()
        {
            NameDataBase = "test",
            NameMediaCollection = "Media",
            NameMediaBasketCollection = "MediaBasket",
            NameUserCollection = "Users"
        }), new MongoClient(_mongoDbContainer.GetConnectionString()));
    }

    public IMapper GetMapper()
    {
        var typeAdapterConfig = new TypeAdapterConfig
        {
            AllowImplicitSourceInheritance = true,
            AllowImplicitDestinationInheritance = true
        };
        Assembly applicationAssembly = Assembly.GetAssembly(typeof(StorageRegistry))!;
        typeAdapterConfig.Scan(applicationAssembly);

        return new Mapper(typeAdapterConfig);
    }

    public virtual async Task InitializeAsync()
    {
        await _mongoDbContainer.StartAsync();
        
        await _mongoDbContainer.ExecScriptAsync($"rs.initiate({{_id:'rs0',members:[{{_id:0,host:'host.docker.internal:{_mongoDbContainer.GetMappedPublicPort(27017)}'}}]}})");
        
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
            db.MediaBasket.createIndex({userId: 1, mediaId: 1},{ unique: true } );

            """;
        
        await _mongoDbContainer.ExecScriptAsync(initializationScript);
    }

    public virtual async Task DisposeAsync()
    {
        await _mongoDbContainer.DisposeAsync();
    }
}