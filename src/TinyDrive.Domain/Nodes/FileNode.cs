namespace TinyDrive.Domain.Nodes;

public sealed class FileNode : Node
{
    public string ContentType { get; private set; } = string.Empty;

    public long Size { get; private set; }

    public FileNode()
    {
    }

    public FileNode(Guid id, Guid? parentId, string name, string contentType, long size) : base(id, parentId, name)
    {
        ContentType = contentType;
        Size = size;
    }
}