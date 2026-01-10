using TinyDrive.SharedKernel;

namespace TinyDrive.Domain.Nodes;

public sealed class Node : Entity
{
    public string Name { get; init; }
    public string? Extension { get; init; }
    public string? ContentType { get; init; }
    public long? Size { get; init; }
    public Ulid? ParentId { get; init; }
    public bool IsFolder { get; init; }

    public DateTime CreatedAtUtc { get; init; }
    public DateTime? LastModifiedAtUtc { get; init; }

    private Node()
    {
    }

    public static Node NewFile(string name, string contentType, long size, DateTime createdAtUtc,
        Ulid? parentId = null) => new()
    {
        Id = Ulid.NewUlid(),
        Name = name,
        ContentType = contentType,
        Size = size,
        ParentId = parentId,
        IsFolder = false,
        CreatedAtUtc = createdAtUtc
    };

    public static Node NewFolder(string name, DateTime createdAtUtc, Ulid? parentId = null) => new()
    {
        Id = Ulid.NewUlid(),
        Name = name,
        Size = 0,
        ParentId = parentId,
        IsFolder = true,
        CreatedAtUtc = createdAtUtc,
    };
}
