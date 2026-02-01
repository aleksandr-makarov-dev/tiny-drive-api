using ErrorOr;
using MediatR;

namespace TinyDrive.Features.Features.Nodes.EmptyTrash;

public sealed record EmptyTrashCommand() : IRequest<ErrorOr<Success>>;
