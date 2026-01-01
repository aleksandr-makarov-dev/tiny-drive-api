namespace TinyDrive.Domain.Nodes;

public sealed class FileNode : Node
{
    public string ContentType { get; init; } = string.Empty;

    public long Size { get; init; }
}
