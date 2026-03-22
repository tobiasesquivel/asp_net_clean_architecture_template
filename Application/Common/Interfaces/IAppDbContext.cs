using Domain.Models.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces;

public interface IAppDbContext
{
    DbSet<User> AppUsers { get; }
}