using MovieHub.Storage.Entities;

namespace MovieHub.KeycloakConsumer;

public class CreateUserStorage(MongoDbContext dbContext) : ICreateUserStorage
{
    public async Task Create(Guid userId,CancellationToken cancellationToken)
    {
        await dbContext.Users.InsertOneAsync(new User()
        {
            Id = userId
        }, null, cancellationToken);
    }
}