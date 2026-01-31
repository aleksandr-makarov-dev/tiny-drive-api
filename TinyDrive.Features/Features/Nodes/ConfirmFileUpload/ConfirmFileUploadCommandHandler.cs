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

namespace TinyDrive.Features.Features.Nodes.ConfirmFileUpload;

public sealed class ConfirmFileUploadCommandHandler(
	ApplicationDbContext dbContext,
	IAmazonS3 s3Client,
	ILogger<ConfirmFileUploadCommandHandler> logger)
	: IRequestHandler<ConfirmFileUploadCommand, ErrorOr<Success>>
{
	public async Task<ErrorOr<Success>> Handle(ConfirmFileUploadCommand request, CancellationToken cancellationToken)
	{
		// Check if File exists
		var file = await FindFileByIdAsync(request.FileId, cancellationToken: cancellationToken);

		if (file is null)
		{
			return NodeErrors.FileNotFound(request.FileId);
		}

		// Check if File already confirmed
		if (file.UploadStatus == UploadStatus.Uploaded)
		{
			return Result.Success;
		}

		// Check if File already failed
		if (file.UploadStatus == UploadStatus.Failed)
		{
			logger.LogWarning("Attempting to confirm a file upload but the file upload marked as failed.");

			return NodeErrors.CannotConfirmFailedUpload();
		}

		// Check if File fully uploaded to s3 bucket
		try
		{
			var getObjectMetadataRequest = new GetObjectMetadataRequest
			{
				BucketName = S3.BucketName,
				Key = file.ObjectKey
			};

			var getObjectMetadataResult =
				await s3Client.GetObjectMetadataAsync(getObjectMetadataRequest, cancellationToken: cancellationToken);

			if (file.Size < getObjectMetadataResult.ContentLength)
			{
				logger.LogWarning("The file upload has not completed successfully.");
				return NodeErrors.FileNotFullyUploaded();
			}

			file.UploadStatus = UploadStatus.Uploaded;
			await dbContext.SaveChangesAsync(cancellationToken: cancellationToken);

			return Result.Success;
		}
		catch (AmazonS3Exception ex)
		{
			logger.LogError(ex, "An unexpected error occured during getting objectMetadata");

			file.UploadStatus = UploadStatus.Failed;
			await dbContext.SaveChangesAsync(cancellationToken: cancellationToken);

			return NodeErrors.GetUploadedObjectFailed();
		}
	}

	private Task<Node?> FindFileByIdAsync(Guid fileId, CancellationToken cancellationToken)
	{
		return dbContext.Nodes.FirstOrDefaultAsync(x => x.Id == fileId && !x.IsFolder,
			cancellationToken: cancellationToken);
	}
}
