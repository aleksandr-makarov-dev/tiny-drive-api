using MediatR;
using TinyDrive.SharedKernel;

namespace TinyDrive.Application.Nodes.CreateFolder;

public record CreateFolderCommand(string Name, Ulid? ParentId = null) : IRequest<Result<Ulid>>;
