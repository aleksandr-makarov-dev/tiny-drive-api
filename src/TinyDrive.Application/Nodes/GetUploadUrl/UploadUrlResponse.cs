namespace TinyDrive.Application.Nodes.GetUploadUrl;

public sealed class UploadUrlResponse
{
    public string Url { get; init; }

    public Dictionary<string, string> Fields { get; init; } = new();

    public DateTime ExpiresOnUtc { get; init; }
}
