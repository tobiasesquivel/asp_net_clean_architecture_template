using Application.Common.Interfaces;
using Cortex.Mediator;
using Domain.Models.Aggregates;
using Domain.Models.Common;
using Infrastructure.Persistance.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Persistance;

public class AppDbContext : IdentityDbContext<AuthUser>, IAppDbContext, IUnitOfWork
{
    private readonly IMediator _publisher;
    public AppDbContext(DbContextOptions options, IMediator publisher) : base(options)
    {
        _publisher = publisher;
    }

    public DbSet<User> AppUsers => Set<User>();


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }

    public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        var domainEntities = ChangeTracker
            .Entries<AggregateRoot<IHasDomainEvents>>()
            .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Count != 0)
            .ToList();

        var domainEvents = domainEntities
            .SelectMany(x => x.Entity.DomainEvents)
            .ToList();

        domainEntities.ForEach(entity => entity.Entity.ClearDomainEvents());

        var result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);

        foreach (var domainEvent in domainEvents)
        {
            await _publisher.PublishAsync(domainEvent, cancellationToken);
        }

        return result;
    }

    public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        return Database.BeginTransactionAsync(cancellationToken);
    }
}