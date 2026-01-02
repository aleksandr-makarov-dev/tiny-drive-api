using MediatR;
using TinyDrive.Application.Abstract;

namespace TinyDrive.Application.Nodes.GetUploadUrl;

public sealed record GetUploadUrlCommand(string FileName, long FileSize, string ContentType)
    : IRequest<Result<UploadUrlResponse>>;
