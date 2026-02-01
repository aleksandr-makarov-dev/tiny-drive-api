using Amazon.S3;
using Amazon.S3.Model;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TinyDrive.Domain.Entities;
using TinyDrive.Domain.Enums;
using TinyDrive.Features.Common.Constants;
using TinyDrive.Features.Common.Errors;
using TinyDrive.Infrastructure.Data;

namespace TinyDrive.Features.Features.Nodes.CreateFileUploadUrl;

public sealed class CreateFileUploadUrlCommandHandler(
	ApplicationDbContext dbContext,
	IAmazonS3 s3Client,
	ILogger<CreateFileUploadUrlCommandHandler> logger)
	: IRequestHandler<CreateFileUploadUrlCommand, ErrorOr<CreateUploadUrlResponse>>
{

	public async Task<ErrorOr<CreateUploadUrlResponse>> Handle(CreateFileUploadUrlCommand request,
		CancellationToken cancellationToken)
	{
		Node? parent = null;

		// Check if Parent Folder exists
		if (request.ParentFolderId.HasValue)
		{
			parent = await FindParentAsync(request.ParentFolderId.Value, cancellationToken: cancellationToken);

			if (parent is null)
			{
				logger.LogWarning("Parent folder with id '{ParentId}' not found", request.ParentFolderId);
				return NodeErrors.ParentFolderNotFound(request.ParentFolderId.Value);
			}
		}

		var file = CreateFile(request.FileName, request.FileSizeBytes, request.ContentType, parent);

		// Check if file with the same Name and Extension exists in Parent Folder
		if (await IsDuplicateFileAsync(file.Name, file.Extension!, file.ParentId, cancellationToken: cancellationToken))
		{
			logger.LogWarning(
				"Duplicate file detected: '{FileName}' in folder '{ParentId}'", file.DisplayName, file.ParentId);

			return NodeErrors.FileAlreadyExists(file.DisplayName);
		}

		var expiresAtUtc = DateTime.Now.Add(S3.PresignedUrlExpiration);

		try
		{
			var createPresignedPostRequest = new CreatePresignedPostRequest
			{
				BucketName = S3.BucketName,
				Key = file.ObjectKey,
				Expires = expiresAtUtc,
				Conditions = [new ContentLengthRangeCondition(1, request.FileSizeBytes + S3.UploadSizeThresholdBytes)]
			};

			var createPresignedPostResult = await s3Client.CreatePresignedPostAsync(createPresignedPostRequest);

			dbContext.Nodes.Add(file);

			await dbContext.SaveChangesAsync(cancellationToken);

			return new CreateUploadUrlResponse(file.Id, createPresignedPostResult.Url, expiresAtUtc,
				createPresignedPostResult.Fields);
		}
		catch (AmazonS3Exception ex)
		{
			logger.LogError(ex, "An unexpected error occured during creating the upload url");

			return NodeErrors.CreateUploadUrlFailed();
		}
	}

	private Task<Node?> FindParentAsync(Guid parentId, CancellationToken cancellationToken)
	{
		return dbContext.Nodes.AsNoTracking().FirstOrDefaultAsync(x => x.Id == parentId && x.IsFolder && !x.IsDeleted,
			cancellationToken: cancellationToken);
	}

	private Task<bool> IsDuplicateFileAsync(string name, string extension, Guid? parentId,
		CancellationToken cancellationToken)
	{
		return dbContext.Nodes.AnyAsync(
			x => EF.Functions.ILike(name, x.Name) &&
			     EF.Functions.ILike(extension, x.Extension!) &&
			     x.ParentId == parentId &&
			     !x.IsFolder &&
			     !x.IsDeleted,
			cancellationToken: cancellationToken);
	}

	private static Node CreateFile(string fileName, long fileSizeBytes, string contentType, Node? parent)
	{
		var name = Path.GetFileNameWithoutExtension(fileName);
		var extension = Path.GetExtension(fileName).TrimStart('.');

		return new Node
		{
			Id = Guid.NewGuid(),
			Name = name,
			Extension = extension,
			Size = fileSizeBytes,
			ContentType = contentType,
			UploadStatus = UploadStatus.Uploading,
			IsFolder = false,
			ParentId = parent?.Id,
			MaterializedPath = parent is null ? $"/{fileName}" : parent.MaterializedPath + fileName,
			CreatedAtUtc = DateTime.UtcNow
		};
	}
}
