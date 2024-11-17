namespace MovieHub.Domain.BackgroundServices.CreateRegisteredUser;

public interface ICreateSynchronizationUserStorage
{
    public Task Create(Guid synchronizationUserId, CancellationToken cancellationToken);
}