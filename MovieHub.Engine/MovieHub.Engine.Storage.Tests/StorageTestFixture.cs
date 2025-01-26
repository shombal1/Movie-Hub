using System.Reflection;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MovieHub.Engine.Storage.Mapping;
using Testcontainers.MongoDb;

namespace MovieHub.Engine.Storage.Tests;

public class StorageTestFixture : IAsyncLifetime
{
    private readonly MongoDbContainer _mongoDbContainer = new MongoDbBuilder().Build();

    public MovieHubDbContext GetMovieHubDbContext()
    {
        return new MovieHubDbContext(new OptionsWrapper<MongoDbConfigure>(new MongoDbConfigure()
        {
            NameDataBase = "MovieHub",
            NameMediaCollection = "Media",
            NameMovieBasketCollection = "MovieBasket",
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
    }

    public virtual async Task DisposeAsync()
    {
        await _mongoDbContainer.DisposeAsync();
    }
}