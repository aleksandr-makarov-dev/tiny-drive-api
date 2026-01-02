using MediatR;
using Microsoft.AspNetCore.Mvc;
using TinyDrive.API.Extensions;
using TinyDrive.API.Infrastructure;
using TinyDrive.Application.Abstract;
using TinyDrive.Application.Nodes.GetFileUploadUrl;

namespace TinyDrive.API.Endpoints.Files;

public class GetFileUploadUrlEndpoint : IEndpoint
{
    private sealed class Request
    {
        public string FileName { get; init; }

        public long FileSize { get; init; }

        public string ContentType { get; init; }
    }

    public void MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapPost("api/files/upload-url",
            async ([FromBody] Request request, ISender sender, CancellationToken cancellationToken) =>
            {
                var command = new GetFileUploadUrlCommand(
                    request.FileName,
                    request.FileSize,
                    request.ContentType
                );

                Result<FileUploadUrlResponse> result = await sender.Send(command, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);
            });
    }
}
