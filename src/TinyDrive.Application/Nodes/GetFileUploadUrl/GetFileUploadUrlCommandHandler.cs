using MediatR;
using Microsoft.Extensions.Logging;
using TinyDrive.Application.Abstract;
using TinyDrive.Application.Abstract.Data;
using TinyDrive.Application.Abstract.Data.Repositories;
using TinyDrive.Application.Abstract.Storage;
using TinyDrive.Application.Abstract.Storage.Models;
using TinyDrive.Domain.Abstract;
using TinyDrive.Domain.Nodes;

namespace TinyDrive.Application.Nodes.GetFileUploadUrl;

internal sealed class
    GetFileUploadUrlCommandHandler(
        IUnitOfWork unitOfWork,
        INodeRepository nodeRepository,
        IDateTimeProvider dateTimeProvider,
        IObjectStorage objectStorage,
        ILogger<GetFileUploadUrlCommandHandler> logger)
    : IRequestHandler<GetFileUploadUrlCommand, Result<FileUploadUrlResponse>>
{
    public async Task<Result<FileUploadUrlResponse>> Handle(GetFileUploadUrlCommand request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating upload url for file {FileName}.", request.Name);

        if (request.ParentId is not null)
        {
            Node? parent = await nodeRepository.FindByIdAsync(request.ParentId.Value, cancellationToken);

            if (parent is null)
            {
                logger.LogWarning("Parent node not found.");

                return Result.Failure<FileUploadUrlResponse>(NodeErrors.ParentNotFound(request.ParentId.Value));
            }

            if (!parent.IsFolder)
            {
                logger.LogWarning("Parent is not a folder.");

                return Result.Failure<FileUploadUrlResponse>(NodeErrors.ParentMustBeFolder(request.ParentId.Value));
            }
        }

        var file = Node.NewFile(
            request.Name,
            request.ContentType,
            request.Size,
            dateTimeProvider.UtcNow,
            request.ParentId
        );

        // TODO: It might be better to check only among successfully uploaded files.
        bool isDuplicate =
            await nodeRepository.ExistsAsync(file.Name, file.Extension, file.ParentId, cancellationToken);

        if (isDuplicate)
        {
            logger.LogWarning("Duplicate folder {FolderName}.", file.DisplayName);

            return Result.Failure<FileUploadUrlResponse>(NodeErrors.Duplicate(file.DisplayName, request.ParentId));
        }

        try
        {
            PresignedPostUrlData presignedUrlDate = await objectStorage.GetPresignedPostUrlAsync(
                file.ObjectKey,
                file.Size,
                file.ContentType!,
                cancellationToken
            );

            file.UploadStatus = UploadStatus.Uploading;

            nodeRepository.Add(file);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            var response = new FileUploadUrlResponse
            {
                Id = file.Id,
                UploadUrl = presignedUrlDate.UploadUrl,
                ExpiresAtUtc = presignedUrlDate.ExpiresAtUtc,
                Fields = presignedUrlDate.Fields,
            };

            return Result.Success(response);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to create upload URL for file {FileName}", request.Name);

            return Result.Failure<FileUploadUrlResponse>(ObjectStorageErrors.UploadUrlCreationFailed());
        }
    }
}
