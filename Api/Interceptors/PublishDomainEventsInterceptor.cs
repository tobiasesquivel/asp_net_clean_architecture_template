using api.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Wolverine;

namespace api.Interceptors;

public class PublishDomainEventsInterceptor : SaveChangesInterceptor
{
    private readonly IMessageBus _messageBus;

    public PublishDomainEventsInterceptor(IMessageBus messageBus)
    {
        _messageBus = messageBus;
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        return base.SavingChanges(eventData, result);
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        await PublishDomainEvents(eventData.Context);
        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private async Task PublishDomainEvents(DbContext? dbContext)
    {
        if (dbContext is null) return;
        var entitiesWithEvents = dbContext.ChangeTracker
                                            .Entries<IHasDomainEvents>()
                                            .Where(entry => entry.Entity.DomainEvents.Any())
                                            .Select(entry => entry.Entity)
                                            .ToList();

        var domainEvents = entitiesWithEvents
                                            .SelectMany(entity => entity.DomainEvents)
                                            .ToList();


        entitiesWithEvents.ForEach(entity => entity.ClearDomainEvents());
        foreach (var domainEvent in domainEvents)
        {
            await _messageBus.PublishAsync((dynamic)domainEvent);
        }
    }
}