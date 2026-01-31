using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TinyDrive.Domain.Entities;
using TinyDrive.Features.Common.Errors;
using TinyDrive.Infrastructure.Data;

namespace TinyDrive.Features.Features.Nodes.CreateFolder;

internal sealed class CreateFolderCommandHandler(ApplicationDbContext dbContext)
	: IRequestHandler<CreateFolderCommand, ErrorOr<Guid>>
{

	public async Task<ErrorOr<Guid>> Handle(CreateFolderCommand request, CancellationToken cancellationToken)
	{
		if (request.ParentFolderId.HasValue &&
		    !await ParentFolderExistsAsync(request.ParentFolderId.Value, cancellationToken: cancellationToken))
		{
			return NodeErrors.ParentFolderNotFound(request.ParentFolderId.Value);
		}

		if (await IsDuplicateFolderAsync(request.Name, request.ParentFolderId, cancellationToken: cancellationToken))
		{
			return NodeErrors.FolderAlreadyExists(request.Name);
		}

		var folder = CreateFolder(request.Name, request.ParentFolderId);

		dbContext.Nodes.Add(folder);

		await dbContext.SaveChangesAsync(cancellationToken);

		return folder.Id;
	}

	private Task<bool> ParentFolderExistsAsync(Guid parentId, CancellationToken cancellationToken)
	{
		return dbContext.Nodes.AnyAsync(x => x.Id == parentId && x.IsFolder,
			cancellationToken: cancellationToken);
	}

	private Task<bool> IsDuplicateFolderAsync(string name, Guid? parentId, CancellationToken cancellationToken)
	{
		return dbContext.Nodes.AnyAsync(x => EF.Functions.ILike(name, x.Name) && x.ParentId == parentId && x.IsFolder,
			cancellationToken: cancellationToken);
	}

	private static Node CreateFolder(string name, Guid? parentId)
	{
		return new Node
		{
			Id = Guid.NewGuid(),
			Name = name,
			ParentId = parentId,
			IsFolder = true,
			CreatedAtUtc = DateTime.UtcNow
		};
	}
}
