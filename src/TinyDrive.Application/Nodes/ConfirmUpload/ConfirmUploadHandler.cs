using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TinyDrive.Application.Abstract;
using TinyDrive.Application.Abstract.Data;
using TinyDrive.Application.Abstract.Storage;
using TinyDrive.Application.Abstract.Storage.Models;
using TinyDrive.Domain.Nodes;

namespace TinyDrive.Application.Nodes.ConfirmUpload;

internal sealed class ConfirmUploadHandler(
    IApplicationDbContext dbContext,
    IObjectStorage objectStorage,
    ILogger<ConfirmUploadHandler> logger)
    : IRequestHandler<ConfirmUploadCommand, Result>
{
    public async Task<Result> Handle(
        ConfirmUploadCommand request,
        CancellationToken cancellationToken)
    {
        FileNode? file = await dbContext
            .Nodes
            .OfType<FileNode>()
            .FirstOrDefaultAsync(
                x => x.Id == request.Id,
                cancellationToken: cancellationToken);

        if (file is null)
        {
            return Result.Failure(Error.NotFound(
                "Nodes.NotFound",
                $"File with id '{request.Id}' was not found."));
        }

        // If upload status is Uploaded then return success immediately
        if (file.UploadStatus == NodeUploadStatus.Uploaded)
        {
            return Result.Success();
        }

        string key = $"{file.Id}{Path.GetExtension(file.Name)}";

        try
        {
            ObjectAttributesData statsObject =
                await objectStorage.GetObjectStatsAsync(key);

            if (file.Size != statsObject.ObjectSize)
            {
                return Result.Failure(Error.Conflict(
                    "Nodes.SizeMismatch",
                    $"Uploaded file size mismatch. Expected {file.Size} bytes, but received {statsObject.ObjectSize} bytes."));
            }

            // set upload status Uploaded
            file.UploadStatus = NodeUploadStatus.Uploaded;

            await dbContext.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch (Exception ex)
        {
            // set upload status Failed
            file.UploadStatus = NodeUploadStatus.Failed;

            await dbContext.SaveChangesAsync(cancellationToken);

            logger.LogError(
                ex,
                "Failed to confirm upload for file with id {FileId}",
                file.Id);

            return Result.Failure(Error.Conflict(
                "Nodes.Failed",
                $"Failed to confirm upload for file '{file.Name}'. Please try again later."));
        }
    }
}
