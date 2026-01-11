using MediatR;
using TinyDrive.SharedKernel;

namespace TinyDrive.Application.Nodes.GetFileUploadUrl;

public sealed record GetFileUploadUrlCommand(string Name, long Size, string ContentType, Ulid? ParentId)
    : IRequest<Result<FileUploadUrlResponse>>;
