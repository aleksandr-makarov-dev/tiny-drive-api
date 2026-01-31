namespace TinyDrive.Features.Features.Nodes.CreateFileUploadUrl;

public sealed record CreateUploadUrlResponse(
	Guid FileId,
	string UploadUrl,
	DateTime ExpiresAtUtc,
	IReadOnlyDictionary<string, string> FormFields);
