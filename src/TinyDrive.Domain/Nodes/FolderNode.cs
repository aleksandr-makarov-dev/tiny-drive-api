namespace TinyDrive.Domain.Nodes;

public sealed class FolderNode : Node
{
    public FolderNode()
    {
    }

    public FolderNode(Guid id, Guid? parentId, string name)
        : base(id, parentId, name)
    {
    }
}