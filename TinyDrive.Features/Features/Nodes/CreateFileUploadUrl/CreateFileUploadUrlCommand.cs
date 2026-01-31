using ErrorOr;
using MediatR;

namespace TinyDrive.Features.Features.Nodes.CreateFileUploadUrl;

public sealed record CreateFileUploadUrlCommand(
	string FileName,
	long FileSizeBytes,
	string ContentType,
	Guid? ParentFolderId) : IRequest<ErrorOr<CreateUploadUrlResponse>>;
