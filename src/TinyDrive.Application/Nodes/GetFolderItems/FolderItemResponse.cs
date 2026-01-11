namespace TinyDrive.Application.Nodes.GetFolderItems;

public sealed class FolderItemResponse
{
    public Ulid Id { get; init; }
    
    public string Name { get; init; }
    
    public string? ContentType { get; init; }
    
    public long? Size { get; init; }
    
    public Ulid? ParentId { get; init; }
    
    public bool IsFolder { get; init; }
    
    public DateTime CreatedAtUtc { get; init; }
    
    public DateTime? LastModifiedAtUtc { get; init; }
}
