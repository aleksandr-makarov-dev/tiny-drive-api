namespace TinyDrive.Application.Nodes.GetUploadUrl;

public sealed class UploadUrlResponse
{
    public Guid Id { get; init; }
    public string Url { get; init; }

    public Dictionary<string, string> Fields { get; init; } = new();

    public DateTime ExpiresAtUtc { get; init; }
}
