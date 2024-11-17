using MovieHub.Domain.Models;

namespace MovieHub.Domain.UseCases;

public interface IGetUserStorage
{
    public Task<User> GetUser(Guid id);

    public Task<bool> UserExists(Guid id);
}