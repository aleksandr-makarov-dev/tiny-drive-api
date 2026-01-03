using MediatR;
using TinyDrive.Application.Abstract;

namespace TinyDrive.Application.Nodes.GetFolderItems;

public sealed record GetFolderItemsQuery(Guid ParentId) : IRequest<Result<PagedResult<FolderItemResponse>>>;
