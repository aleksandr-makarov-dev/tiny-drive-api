using TinyDrive.Domain.Abstract;

namespace TinyDrive.Domain.Nodes;

public sealed class Node : Entity
{
    public string Name { get; init; }
    public string? Extension { get; init; }

    public string? ContentType { get; init; }
    public long Size { get; init; }
    public Ulid? ParentId { get; init; }
    public bool IsFolder { get; init; }

    public UploadStatus? UploadStatus { get; set; }

    public DateTime CreatedAtUtc { get; init; }
    public DateTime? LastModifiedAtUtc { get; init; }

    public string FullName => Extension is null or "" ? Name : $"{Name}.{Extension}";

    private Node()
    {
    }

    public static Node NewFile(string name, string contentType, long size, DateTime createdAtUtc,
        Ulid? parentId = null)
    {
        string fileName = Path.GetFileNameWithoutExtension(name);
        string extension = Path.GetExtension(name).TrimStart('.');

        return new Node
        {
            Id = Ulid.NewUlid(),
            Name = fileName,
            Extension = extension,
            ContentType = contentType,
            Size = size,
            ParentId = parentId,
            IsFolder = false,
            UploadStatus = Nodes.UploadStatus.Init,
            CreatedAtUtc = createdAtUtc
        };
    }

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
