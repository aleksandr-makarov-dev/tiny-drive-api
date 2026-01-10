using MediatR;
using TinyDrive.SharedKernel;

namespace TinyDrive.Application.Nodes.GetFolderItems;

public sealed record GetFolderItemsQuery(Ulid? ParentId) : IRequest<Result<PagedResult<FolderItemResponse>>>;
