using api.Models;
using api.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace api.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _dbContext;

    public UserRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Add(User user)
    {
        Console.WriteLine(JsonConvert.SerializeObject(user, Formatting.Indented));
        _dbContext.AppUsers.Add(user);
    }

    public Task<bool> ExistsAsync(string userId)
    {
        return _dbContext.Users.AnyAsync(u => u.Id == userId);
    }
}