namespace TinyDrive.Domain.Nodes;

public abstract class Node
{
    public Guid Id { get; private set; }

    public Guid? ParentId { get; private set; }

    public string Name { get; private set; } = string.Empty;


    protected Node()
    {
    }

    protected Node(Guid id, Guid? parentId, string name)
    {
        Id = id;
        ParentId = parentId;
        Name = name;
    }
}