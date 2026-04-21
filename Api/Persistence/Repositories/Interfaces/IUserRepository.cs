using api.Models;

namespace api.Persistence.Repositories.Interfaces;

public interface IUserRepository
{
    public void Add(User user);
    public Task<bool> ExistsAsync(string userId);
}