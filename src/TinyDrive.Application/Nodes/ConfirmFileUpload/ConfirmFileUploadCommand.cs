using MediatR;
using TinyDrive.Application.Abstract;

namespace TinyDrive.Application.Nodes.ConfirmFileUpload;

public sealed record ConfirmFileUploadCommand(Ulid FileId) : IRequest<Result>;
