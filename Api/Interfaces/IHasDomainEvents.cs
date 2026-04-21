namespace api.Interfaces;

public interface IHasDomainEvents
{
    public IReadOnlyList<IDomainEvent> DomainEvents => [];

    public void AddDomainEvent(IDomainEvent domainEvent);

    public void ClearDomainEvents();
}