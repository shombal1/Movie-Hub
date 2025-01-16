
namespace MovieHub.KeycloakConsumer;

public interface ICreateUserStorage
{
    public Task Create(Guid userId,CancellationToken cancellationToken);
}