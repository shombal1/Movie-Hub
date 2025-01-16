
namespace MovieHub.UserSync.KeycloakConsumer;

public interface ICreateUserStorage
{
    public Task Create(Guid userId,CancellationToken cancellationToken);
}