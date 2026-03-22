using Domain.Models.Aggregates;
using ErrorOr;

namespace Application.Auth.Persistence.Repositories;

public interface IUserRepository
{
    User AddAsync(User user);
}