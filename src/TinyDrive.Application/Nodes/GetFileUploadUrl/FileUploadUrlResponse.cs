namespace TinyDrive.Application.Nodes.GetFileUploadUrl;

public sealed class FileUploadUrlResponse
{
    public Ulid Id { get; init; }

    public string UploadUrl { get; init; }

    public DateTime ExpiresAtUtc { get; init; }

    public Dictionary<string, string> Fields { get; init; }
}
