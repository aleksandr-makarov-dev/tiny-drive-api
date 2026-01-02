using MediatR;
using TinyDrive.Application.Abstract;

namespace TinyDrive.Application.Nodes.ConfirmUpload;

public sealed record ConfirmUploadCommand(Guid Id) : IRequest<Result>;
