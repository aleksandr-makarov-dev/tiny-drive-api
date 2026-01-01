using MediatR;
using TinyDrive.Application.Abstract;
using TinyDrive.Domain.Nodes;
using TinyDrive.Domain.Nodes.Events;

namespace TinyDrive.Application.Nodes.GetFileUploadUrl;

internal sealed class
    GetFileUploadUrlCommandHandler : IRequestHandler<GetFileUploadUrlCommand, Result<FileUploadUrlResponse>>
{
    public async Task<Result<FileUploadUrlResponse>> Handle(GetFileUploadUrlCommand request,
        CancellationToken cancellationToken)
    {
        await Task.Delay(50, cancellationToken);

        var file = new FileNode
        {
            Id = Guid.NewGuid(),
            Name = request.FileName,
            Size = request.FileSize,
            ContentType = request.ContentType,
        };

        file.Raise(new FileCreatedDomainEvent(file.Id, file.Name));

        return Result.Success(new FileUploadUrlResponse
        {
            Url = "https://example.com/upload-file",
            CreatedOnUtc = DateTime.UtcNow,
            ExpiresOnUtc = DateTime.UtcNow.AddMinutes(30)
        });
    }
}
