using System.Text.Json;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MovieHub.Engine.Storage.Entities;

namespace MovieHub.Engine.Storage;

// registry how scope
public class MovieHubDbContext : IAsyncDisposable
{
    private IMongoDatabase Database { get; }
    
    public IMongoCollection<MovieBasketEntity> MovieBaskets { get; }
    public IMongoCollection<MediaEntity> Media { get; }
    public IMongoCollection<UserEntity> Users { get; }

    public IClientSessionHandle CurrentSession { get; }

    public MovieHubDbContext(IOptions<MongoDbConfigure> options,IMongoClient mongoClient)
    {
        MongoDbConfigure configure = options.Value;

        Database = mongoClient.GetDatabase(configure.NameDataBase);

        CurrentSession = mongoClient.StartSession();
        
        MovieBaskets = Database.GetCollection<MovieBasketEntity>(configure.NameMovieBasketCollection);
        Media = Database.GetCollection<MediaEntity>(configure.NameMediaCollection);
        Users = Database.GetCollection<UserEntity>(configure.NameUserCollection);
    }

    public async ValueTask DisposeAsync()
    {
        CurrentSession.Dispose();
    }
}