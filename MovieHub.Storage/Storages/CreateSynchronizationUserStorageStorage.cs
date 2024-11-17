using MovieHub.Domain.BackgroundServices.CreateRegisteredUser;
using MovieHub.Storage.Entities;

namespace MovieHub.Storage.Storages;

public class CreateSynchronizationUserStorageStorage(MovieHubDbContext dbContext) : ICreateSynchronizationUserStorage
{
    public async Task Create(Guid synchronizationUserId, CancellationToken cancellationToken)
    {
        await dbContext.Users.AddAsync(new User()
        {
            Id = synchronizationUserId
        }, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}