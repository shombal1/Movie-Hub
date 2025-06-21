using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace MovieHub.MediaCompressor.Mongo;

public class MongoDbContext
{
    private IMongoDatabase Database { get; }
    
    public IMongoCollection<MovieRequestEntity> MovieRequests { get; }
    
    public IClientSessionHandle CurrentSession { get; }
    
    public MongoDbContext(IMongoClient mongoClient, IOptions<MongoDbConfigure> options)
    {
        MongoDbConfigure configure = options.Value;
        
        CurrentSession = mongoClient.StartSession();
        
        Database = mongoClient.GetDatabase(configure.NameDataBase);
        
        MovieRequests = Database.GetCollection<MovieRequestEntity>(configure.NameMovieRequestCollection);
    }
    
    public ValueTask DisposeAsync()
    {
        CurrentSession.Dispose();
        return ValueTask.CompletedTask;
    }
}