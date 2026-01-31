namespace TinyDrive.Features.Features.Nodes.CreateFileUploadUrl;

public sealed record CreateFileUploadUrlRequest(
	string FileName,
	long FileSizeBytes,
	string ContentType,
	Guid? ParentFolderId);
