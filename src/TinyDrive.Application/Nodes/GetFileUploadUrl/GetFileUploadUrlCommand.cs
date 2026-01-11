using MediatR;
using TinyDrive.Application.Abstract;

namespace TinyDrive.Application.Nodes.GetFileUploadUrl;

public sealed record GetFileUploadUrlCommand(string Name, long Size, string ContentType, Ulid? ParentId)
    : IRequest<Result<FileUploadUrlResponse>>;
