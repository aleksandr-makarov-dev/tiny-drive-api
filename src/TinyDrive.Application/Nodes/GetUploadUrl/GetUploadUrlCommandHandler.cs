using MediatR;
using Microsoft.Extensions.Logging;
using TinyDrive.Application.Abstract;
using TinyDrive.Application.Abstract.Data;
using TinyDrive.Application.Abstract.Storage;
using TinyDrive.Application.Abstract.Storage.Models;
using TinyDrive.Domain.Nodes;

namespace TinyDrive.Application.Nodes.GetUploadUrl;

internal sealed class
    GetUploadUrlCommandHandler(
        IApplicationDbContext dbContext,
        IObjectStorage objectStorage,
        ILogger<GetUploadUrlCommandHandler> logger)
    : IRequestHandler<GetUploadUrlCommand, Result<UploadUrlResponse>>
{
    public async Task<Result<UploadUrlResponse>> Handle(
        GetUploadUrlCommand request,
        CancellationToken cancellationToken)
    {
        var file = new FileNode
        {
            Id = Guid.NewGuid(),
            Name = request.FileName,
            Size = request.FileSize,
            ContentType = request.ContentType,
            UploadStatus = NodeUploadStatus.Uploading
        };

        dbContext.Nodes.Add(file);
        await dbContext.SaveChangesAsync(cancellationToken);

        try
        {
            string key = $"{file.Id}{Path.GetExtension(request.FileName)}";

            PresignedPostData response =
                await objectStorage.CreatePresignedPostAsync(
                    key,
                    file.Size,
                    file.ContentType);

            return Result.Success(new UploadUrlResponse
            {
                Url = response.Url,
                Fields = response.Fields,
                ExpiresOnUtc = response.ExpiresOnUtc,
            });
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Failed to generate presigned upload URL for file with id {FileId}",
                file.Id);

            return Result.Failure<UploadUrlResponse>(Error.Conflict(
                "Nodes.Failed",
                $"Unable to prepare upload for file '{file.Name}'. Please try again later."));
        }
    }
}
