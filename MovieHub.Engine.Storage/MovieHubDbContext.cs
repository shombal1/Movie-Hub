using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MovieHub.Engine.Storage.Entities;

namespace MovieHub.Engine.Storage;

// registry how scope
public class MovieHubDbContext : IAsyncDisposable
{
    private IMongoDatabase Database { get; }
    
    public IMongoCollection<MediaBasketEntity> MediaBasket { get; }
    public IMongoCollection<MediaEntity> Media { get; }
    public IMongoCollection<UserEntity> Users { get; }
    public IMongoCollection<SeasonEntity> Seasons { get; }
    public IMongoCollection<AdditionMediaInfoEntity> AdditionMediaInfo { get; }
    public IMongoCollection<DomainEvent> DomainEvents { get; }
    public IMongoCollection<MovieRequestEntity> MovieRequests { get; }

    public IClientSessionHandle CurrentSession { get; }

    public MovieHubDbContext(IOptions<MongoDbConfigure> options,IMongoClient mongoClient)
    {
        MongoDbConfigure configure = options.Value;

        Database = mongoClient.GetDatabase(configure.NameDataBase);

        CurrentSession = mongoClient.StartSession();
        
        MediaBasket = Database.GetCollection<MediaBasketEntity>(configure.NameMediaBasketCollection);
        Media = Database.GetCollection<MediaEntity>(configure.NameMediaCollection);
        Users = Database.GetCollection<UserEntity>(configure.NameUserCollection);
        Seasons = Database.GetCollection<SeasonEntity>(configure.NameSeasonCollection);
        AdditionMediaInfo = Database.GetCollection<AdditionMediaInfoEntity>(configure.NameAdditionMediaInfoCollection);
        DomainEvents = Database.GetCollection<DomainEvent>(configure.NameDomainEventCollection);
        MovieRequests = Database.GetCollection<MovieRequestEntity>(configure.NameMovieRequestCollection);
    }

    public ValueTask DisposeAsync()
    {
        CurrentSession.Dispose();
        return ValueTask.CompletedTask;
    }
}