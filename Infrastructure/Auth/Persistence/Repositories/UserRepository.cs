
using Application.Auth.Persistence.Repositories;
using Domain.Models.Aggregates;
using Infrastructure.Persistance;

namespace Infrastructure.Auth.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _dbContext;

    public UserRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public User AddAsync(User user)
    {
        var entry = _dbContext.AppUsers.Add(user);
        return entry.Entity;
    }

    public Task<User> GetByIdAsync(string id)
    {
        throw new NotImplementedException();
    }
}