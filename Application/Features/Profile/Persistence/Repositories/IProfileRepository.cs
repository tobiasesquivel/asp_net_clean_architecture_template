using Domain.Models.Entities;

namespace Application.Features.Profile.Persistence.Repositories;

public interface IProfileRepository
{
    Task<UserProfile> GetByIdAsync(string id);
    Task<UserProfile> AddAsync(UserProfile profile);
}