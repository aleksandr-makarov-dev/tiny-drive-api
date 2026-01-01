using MediatR;
using TinyDrive.Application.Abstract;

namespace TinyDrive.Application.Nodes.GetFileUploadUrl;

public sealed record GetFileUploadUrlCommand(string FileName, long FileSize, string ContentType)
    : IRequest<Result<FileUploadUrlResponse>>;
