using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TinyDrive.Features.Common.Errors;
using TinyDrive.Features.Common.Extensions;
using TinyDrive.Features.Common.Mappings;
using TinyDrive.Features.Common.Models;
using TinyDrive.Infrastructure.Data;

namespace TinyDrive.Features.Features.Nodes.GetFolderItems;

public sealed class GetFolderItemsQueryHandler(ApplicationDbContext dbContext)
	: IRequestHandler<GetFolderItemsQuery, ErrorOr<PaginatedList<FolderItem>>>
{

	public async Task<ErrorOr<PaginatedList<FolderItem>>> Handle(GetFolderItemsQuery request,
		CancellationToken cancellationToken)
	{
		if (request.ParentId.HasValue &&
		    !await ParentFolderExistsAsync(request.ParentId.Value, cancellationToken: cancellationToken))
		{
			return NodeErrors.ParentFolderNotFound();
		}

		return await dbContext.Nodes
			.Where(x => x.ParentId == request.ParentId)
			.OrderByDescending(x => x.IsFolder)
			.ThenBy(x => x.Name)
			.ProjectToFolderItems()
			.PaginatedListAsync(request.PageNumber, request.PageSize, cancellationToken: cancellationToken);
	}

	private Task<bool> ParentFolderExistsAsync(Guid parentId, CancellationToken cancellationToken)
	{
		return dbContext.Nodes.AnyAsync(x => x.Id == parentId && x.IsFolder,
			cancellationToken: cancellationToken);
	}
}
