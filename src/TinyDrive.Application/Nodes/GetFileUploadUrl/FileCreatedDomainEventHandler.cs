using MediatR;
using TinyDrive.Domain.Nodes.Events;

namespace TinyDrive.Application.Nodes.GetFileUploadUrl;

internal sealed class FileCreatedDomainEventHandler : INotificationHandler<FileCreatedDomainEvent>
{
    public Task Handle(FileCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
