using MediatR;
using TinyDrive.Application.Abstract;
using TinyDrive.Application.Abstract.Data;
using TinyDrive.Application.Abstract.Storage;
using TinyDrive.Application.Abstract.Storage.Models;
using TinyDrive.Domain.Nodes;
using TinyDrive.Domain.Nodes.Events;

namespace TinyDrive.Application.Nodes.GetFileUploadUrl;

internal sealed class
    GetFileUploadUrlCommandHandler(IApplicationDbContext dbContext, IObjectStorage objectStorage)
    : IRequestHandler<GetFileUploadUrlCommand, Result<FileUploadUrlResponse>>
{
    public async Task<Result<FileUploadUrlResponse>> Handle(GetFileUploadUrlCommand request,
        CancellationToken cancellationToken)
    {
        var file = new FileNode
        {
            Id = Guid.NewGuid(),
            Name = request.FileName,
            Size = request.FileSize,
            ContentType = request.ContentType,
        };

        file.Raise(new FileCreatedDomainEvent(file.Id, file.Name));

        dbContext.Nodes.Add(file);

        await dbContext.SaveChangesAsync(cancellationToken);

        string key = $"{file.Id}{Path.GetExtension(request.FileName)}";

        PresignedPostData response =
            await objectStorage.CreatePresignedPostAsync(key, file.Size, file.ContentType);


        return Result.Success(new FileUploadUrlResponse
        {
            Url = response.Url,
            Fields = response.Fields,
            ExpiresOnUtc = response.ExpiresOnUtc,
        });
    }
}
