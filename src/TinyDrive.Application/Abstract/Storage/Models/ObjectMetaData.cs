namespace TinyDrive.Application.Abstract.Storage.Models;

public sealed class ObjectMetaData
{
    public long ContentLength { get; init; }

    public string ContentType { get; init; }

    public DateTime? LastModifiedAtUtc { get; init; }
}
