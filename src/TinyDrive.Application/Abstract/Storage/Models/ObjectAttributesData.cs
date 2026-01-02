namespace TinyDrive.Application.Abstract.Storage.Models;

public sealed class ObjectAttributesData
{
    public long? ObjectSize { get; init; }
    
    public string ETag { get; init; }
}
