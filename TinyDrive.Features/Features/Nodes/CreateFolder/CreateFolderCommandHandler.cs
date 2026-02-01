using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TinyDrive.Domain.Entities;
using TinyDrive.Features.Common.Errors;
using TinyDrive.Infrastructure.Data;

namespace TinyDrive.Features.Features.Nodes.CreateFolder;

internal sealed class CreateFolderCommandHandler(
	ApplicationDbContext dbContext,
	ILogger<CreateFolderCommandHandler> logger)
	: IRequestHandler<CreateFolderCommand, ErrorOr<Guid>>
{

	public async Task<ErrorOr<Guid>> Handle(CreateFolderCommand request, CancellationToken cancellationToken)
	{
		Node? parent = null;

		if (request.ParentFolderId.HasValue)
		{
			parent = await FindParentAsync(request.ParentFolderId.Value, cancellationToken: cancellationToken);

			if (parent is null)
			{
				logger.LogWarning("Parent folder with id '{ParentId}' not found", request.ParentFolderId);
				return NodeErrors.ParentFolderNotFound(request.ParentFolderId.Value);
			}
		}

		if (await IsDuplicateFolderAsync(request.Name, request.ParentFolderId, cancellationToken: cancellationToken))
		{
			return NodeErrors.FolderAlreadyExists(request.Name);
		}

		var folder = CreateFolder(request.Name, parent);

		dbContext.Nodes.Add(folder);

		await dbContext.SaveChangesAsync(cancellationToken);

		return folder.Id;
	}

	private Task<Node?> FindParentAsync(Guid parentId, CancellationToken cancellationToken)
	{
		return dbContext.Nodes.AsNoTracking()
			.FirstOrDefaultAsync(x => x.Id == parentId && x.IsFolder && !x.IsDeleted,
				cancellationToken: cancellationToken);
	}

	private Task<bool> IsDuplicateFolderAsync(string name, Guid? parentId, CancellationToken cancellationToken)
	{
		return dbContext.Nodes.AnyAsync(
			x => EF.Functions.ILike(name, x.Name) && x.ParentId == parentId && x.IsFolder && !x.IsDeleted,
			cancellationToken: cancellationToken);
	}

	private static Node CreateFolder(string name, Node? parent)
	{
		return new Node
		{
			Id = Guid.NewGuid(),
			Name = name,
			IsFolder = true,
			ParentId = parent?.Id,
			MaterializedPath = parent is null ? $"/{name}/" : parent.MaterializedPath + name,
			CreatedAtUtc = DateTime.UtcNow
		};
	}
}
