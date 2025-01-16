using MovieHub.Storage.Entities;

namespace MovieHub.UserSync.KeycloakConsumer;

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