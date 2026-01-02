using TinyDrive.Domain.Abstract;

namespace TinyDrive.Domain.Nodes;

public abstract class Node : Entity
{
    public Guid Id { get; init; }

    public Guid? ParentId { get; set; }

    public string Name { get; set; } = string.Empty;

    public NodeUploadStatus UploadStatus { get; set; }
}
