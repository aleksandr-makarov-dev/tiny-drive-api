using ErrorOr;
using MediatR;

namespace TinyDrive.Features.Features.Nodes.ConfirmFileUpload;

public sealed record ConfirmFileUploadCommand(Guid FileId) : IRequest<ErrorOr<Success>>;
