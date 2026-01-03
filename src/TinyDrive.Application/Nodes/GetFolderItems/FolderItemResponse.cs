using TinyDrive.Domain.Nodes;

namespace TinyDrive.Application.Nodes.GetFolderItems;

public sealed class FolderItemResponse
{
    public Guid Id { get; init; }
    
    public string Name { get; init; } = string.Empty;

    public string ContentType { get; init; } = string.Empty;

    public long Size { get; init; }

    public NodeType Type { get; init; }

    public DateTime CreatedAtUtc { get; init; }
}
