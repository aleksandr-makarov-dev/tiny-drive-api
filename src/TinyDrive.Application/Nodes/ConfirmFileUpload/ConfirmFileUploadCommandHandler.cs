using MediatR;
using Microsoft.Extensions.Logging;
using TinyDrive.Application.Abstract;
using TinyDrive.Application.Abstract.Data;
using TinyDrive.Application.Abstract.Data.Repositories;
using TinyDrive.Application.Abstract.Storage;
using TinyDrive.Application.Abstract.Storage.Models;
using TinyDrive.Domain.Nodes;

namespace TinyDrive.Application.Nodes.ConfirmFileUpload;

internal sealed class ConfirmFileUploadCommandHandler(
    IUnitOfWork unitOfWork,
    INodeRepository nodeRepository,
    IObjectStorage objectStorage,
    ILogger<ConfirmFileUploadCommandHandler> logger) : IRequestHandler<ConfirmFileUploadCommand, Result>
{
    public async Task<Result> Handle(ConfirmFileUploadCommand request, CancellationToken cancellationToken)
    {
        Node? file = await nodeRepository.FindByIdAsync(request.FileId, cancellationToken);

        if (ValidateNode(file, request.FileId) is { IsFailure: true } nodeValidationResult)
        {
            return nodeValidationResult;
        }

        if (file.UploadStatus == UploadStatus.Commited)
        {
            return Result.Success();
        }

        try
        {
            ObjectMetaData metaData =
                await objectStorage.GetObjectMetaDataAsync(file.ObjectKey, cancellationToken: cancellationToken);

            if (ValidateUploadedFile(file, metaData) is { IsFailure: true } uploadedFileValidationResult)
            {
                return uploadedFileValidationResult;
            }

            file.UploadStatus = UploadStatus.Commited;

            nodeRepository.Update(file);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch (Exception e)
        {
            file.UploadStatus = UploadStatus.Failed;
            
            nodeRepository.Update(file);
            
            await unitOfWork.SaveChangesAsync(cancellationToken);

            logger.LogError(e, "Failed to confirm file upload for {FileId}", request.FileId);

            return Result.Failure(ObjectStorageErrors.UploadMetadataNotFound(request.FileId));
        }
    }

    private static Result ValidateNode(Node? file, Ulid fileId)
    {
        if (file is null)
        {
            return Result.Failure(NodeErrors.NotFound(fileId));
        }

        return file.IsFolder ? Result.Failure(NodeErrors.MustBeFile(file.Id)) : Result.Success();
    }

    private static Result ValidateUploadedFile(Node file, ObjectMetaData metaData)
    {
        if (file.Size != metaData.ContentLength)
        {
            return Result.Failure(ObjectStorageErrors.SizeMismatch(file.Size, metaData.ContentLength));
        }

        if (!string.Equals(metaData.ContentType, file.ContentType, StringComparison.OrdinalIgnoreCase))
        {
            return Result.Failure(ObjectStorageErrors.ContentTypeMismatch(file.ContentType!, metaData.ContentType));
        }

        return metaData.LastModifiedAtUtc < file.CreatedAtUtc
            ? Result.Failure(ObjectStorageErrors.InvalidUploadTime(file.Id))
            : Result.Success();
    }
}
