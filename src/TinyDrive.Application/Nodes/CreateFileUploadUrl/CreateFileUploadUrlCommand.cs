using MediatR;
using TinyDrive.Application.Abstract;

namespace TinyDrive.Application.Nodes.CreateFileUploadUrl;

public sealed record CreateFileUploadUrlCommand(string Name, long Size, string ContentType, Ulid? ParentId)
    : IRequest<Result<FileUploadUrlResponse>>;
