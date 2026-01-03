namespace TinyDrive.Application.Abstract.Storage.Models;

public sealed class PresignedPostData
{
    public string Url { get; init; }

    public Dictionary<string, string> Fields { get; init; } = new();

    public DateTime ExpiresAtUtc { get; init; }
}
