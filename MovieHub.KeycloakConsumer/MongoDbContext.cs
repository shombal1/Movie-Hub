using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MovieHub.Storage.Entities;

namespace MovieHub.KeycloakConsumer;

public class MongoDbContext
{
    private IMongoDatabase Database { get; }
    
    public IMongoCollection<User> Users { get; }
    
    public MongoDbContext(IMongoClient mongoClient, IOptions<MongoDbConfigure> options)
    {
        MongoDbConfigure configure = options.Value;
        
        Database = mongoClient.GetDatabase(configure.NameDataBase);
        
        Users = Database.GetCollection<User>(configure.NameUserCollection);
    }
}