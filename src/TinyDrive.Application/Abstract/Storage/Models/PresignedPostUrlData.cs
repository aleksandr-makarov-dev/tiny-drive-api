namespace TinyDrive.Application.Abstract.Storage.Models;

public sealed class PresignedPostUrlData
{
    public string UploadUrl { get; init; }

    public DateTime ExpiresAtUtc { get; init; }

    public Dictionary<string, string> Fields { get; init; }
}
