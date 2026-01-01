namespace TinyDrive.Application.Nodes.GetFileUploadUrl;

public sealed class FileUploadUrlResponse
{
    public DateTime CreatedOnUtc { get; init; }

    public DateTime ExpiresOnUtc { get; init; }

    public string Url { get; init; }
}
