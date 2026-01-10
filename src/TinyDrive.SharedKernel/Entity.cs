using MediatR;

namespace TinyDrive.SharedKernel;

public abstract class Entity
{
    private readonly List<INotification> _domainEvents = [];

    public List<INotification> DomainEvents => [.._domainEvents];

    public Ulid Id { get; init; }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    public void Raise(INotification domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
}
