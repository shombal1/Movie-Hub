using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MovieHub.Storage.Entities;

namespace MovieHub.Storage;

// registry how scope
public class MovieHubDbContext : IAsyncDisposable
{
    private IMongoDatabase Database { get; }
    
    public IMongoCollection<MovieBasket> MovieBaskets { get; }
    public IMongoCollection<Movie> Movies { get; }
    public IMongoCollection<User> Users { get; }

    public IClientSessionHandle CurrentSession { get; }

    public MovieHubDbContext(IOptions<MongoDbConfigure> options,IMongoClient mongoClient)
    {
        MongoDbConfigure configure = options.Value;

        Database = mongoClient.GetDatabase(configure.NameDataBase);

        CurrentSession = mongoClient.StartSession();
        
        MovieBaskets = Database.GetCollection<MovieBasket>(configure.NameMovieBasketCollection);
        Movies = Database.GetCollection<Movie>(configure.NameMovieCollection);
        Users = Database.GetCollection<User>(configure.NameUserCollection);
    }

    public async ValueTask DisposeAsync()
    {
        CurrentSession.Dispose();
    }
}