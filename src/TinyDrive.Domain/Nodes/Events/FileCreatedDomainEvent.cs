using MediatR;
using TinyDrive.Domain.Abstract;

namespace TinyDrive.Domain.Nodes.Events;

public sealed record FileCreatedDomainEvent(Guid Id, string FileName): INotification;
