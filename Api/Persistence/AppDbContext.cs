using api.Interceptors;
using api.Interfaces;
using api.Models;
using api.Persistence.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace api.Persistence;

public class AppDbContext : IdentityDbContext<AppIdentityUser>
{
    private readonly PublishDomainEventsInterceptor _publishDomainEventsInterceptor;
    public DbSet<User> AppUsers { get; private set; }
    public DbSet<Post> Posts { get; private set; }
    public AppDbContext(DbContextOptions options, PublishDomainEventsInterceptor publishDomainEventsInterceptor) : base(options)
    {
        _publishDomainEventsInterceptor = publishDomainEventsInterceptor;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder
        .Ignore<List<IDomainEvent>>()
        .ApplyConfigurationsFromAssembly(typeof(DependencyInjection.DependencyInjection).Assembly);
        base.OnModelCreating(builder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_publishDomainEventsInterceptor);
        base.OnConfiguring(optionsBuilder);
    }


}