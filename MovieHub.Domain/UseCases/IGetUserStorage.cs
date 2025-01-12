using MovieHub.Domain.Models;

namespace MovieHub.Domain.UseCases;

public interface IGetUserStorage : IStorage
{
    public Task<User> GetUser(Guid id);

    public Task<bool> UserExists(Guid id);
}