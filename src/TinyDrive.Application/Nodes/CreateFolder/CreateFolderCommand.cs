using MediatR;
using TinyDrive.Application.Abstract;

namespace TinyDrive.Application.Nodes.CreateFolder;

public sealed record CreateFolderCommand(string Name, Ulid? ParentId = null) : IRequest<Result<Ulid>>;
