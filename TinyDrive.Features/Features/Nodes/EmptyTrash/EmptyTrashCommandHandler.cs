using Amazon.S3;
using Amazon.S3.Model;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TinyDrive.Features.Common.Constants;
using TinyDrive.Features.Common.Errors;
using TinyDrive.Infrastructure.Data;

namespace TinyDrive.Features.Features.Nodes.EmptyTrash;

public sealed class EmptyTrashCommandHandler(
	ApplicationDbContext dbContext,
	IAmazonS3 s3Client,
	ILogger<EmptyTrashCommandHandler> logger) : IRequestHandler<EmptyTrashCommand, ErrorOr<Success>>
{

	public async Task<ErrorOr<Success>> Handle(EmptyTrashCommand request, CancellationToken cancellationToken)
	{
		var nodes = await dbContext.Nodes.IgnoreQueryFilters().Where(x => x.IsDeleted)
			.ToListAsync(cancellationToken: cancellationToken);

		try
		{
			var deleteObjectsRequest = new DeleteObjectsRequest
			{
				BucketName = S3.BucketName,
				Objects =
				[
					.. nodes.Where(x => !x.IsFolder).Select(x => new KeyVersion
					{
						Key = x.ObjectKey
					})
				]
			};

			var deleteObjectsResult =
				await s3Client.DeleteObjectsAsync(deleteObjectsRequest, cancellationToken: cancellationToken);

			if (deleteObjectsResult.DeleteErrors?.Count != 0)
			{
				logger.LogWarning("Some objects are failed to be deleted.");
			}

			await dbContext.Nodes.IgnoreQueryFilters().Where(x => x.IsDeleted)
				.ExecuteDeleteAsync(cancellationToken: cancellationToken);

			return Result.Success;
		}
		catch (AmazonS3Exception ex)
		{
			logger.LogError(ex, "An unexpected error occured during deleting objects");

			return NodeErrors.DeleteObjectFailed();
		}
	}
}
