using TinyDrive.Domain.Abstract;

namespace TinyDrive.Domain.Nodes;

public abstract class Node : Entity
{
    public Guid Id { get; init; }

    public Guid? ParentId { get; init; }

    public string Name { get; init; } = string.Empty;
}
