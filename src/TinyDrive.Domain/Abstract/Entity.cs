using MediatR;

namespace TinyDrive.Domain.Abstract;

public abstract class Entity
{
    private readonly List<INotification> _domainEvents = [];

    public List<INotification> DomainEvents => [.._domainEvents];

    public Guid Id { get; init; }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    public void Raise(INotification domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
}
