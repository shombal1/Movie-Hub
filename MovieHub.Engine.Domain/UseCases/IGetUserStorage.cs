using MovieHub.Engine.Domain.Models;

namespace MovieHub.Engine.Domain.UseCases;

public interface IGetUserStorage : IStorage
{
    public Task<User> GetUser(Guid id);

    public Task<bool> UserExists(Guid id);
}