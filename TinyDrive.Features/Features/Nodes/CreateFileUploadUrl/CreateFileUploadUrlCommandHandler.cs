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
		// Check if Parent Folder exists
		if (request.ParentFolderId.HasValue &&
		    !await ParentFolderExistsAsync(request.ParentFolderId.Value, cancellationToken: cancellationToken))
		{
			logger.LogWarning("Parent folder with Id '{ParentId}' not found", request.ParentFolderId);
			return NodeErrors.ParentFolderNotFound();
		}

		var file = CreateFile(request.FileName, request.FileSizeBytes, request.ContentType, request.ParentFolderId);

		// Check if file with the same Name and Extension exists in Parent Folder
		if (await IsDuplicateFileAsync(file.Name, file.Extension!, file.ParentId, cancellationToken: cancellationToken))
		{
			logger.LogWarning(
				"Duplicate file detected: '{FileName}' in folder '{ParentId}'", file.DisplayName, file.ParentId);

			return NodeErrors.FileAlreadyExists();
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

			return NodeErrors.CreateUploadUrlFailure();
		}
	}

	private Task<bool> ParentFolderExistsAsync(Guid parentId, CancellationToken cancellationToken)
	{
		return dbContext.Nodes.AnyAsync(x => x.Id == parentId && x.IsFolder,
			cancellationToken: cancellationToken);
	}

	private Task<bool> IsDuplicateFileAsync(string name, string extension, Guid? parentId,
		CancellationToken cancellationToken)
	{
		return dbContext.Nodes.AnyAsync(
			x => EF.Functions.ILike(name, x.Name) &&
			     EF.Functions.ILike(extension, x.Extension!) &&
			     x.ParentId == parentId &&
			     !x.IsFolder,
			cancellationToken: cancellationToken);
	}

	private static Node CreateFile(string fileName, long fileSizeBytes, string contentType, Guid? parentId)
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
			ParentId = parentId,
			CreatedAtUtc = DateTime.UtcNow
		};
	}

}
