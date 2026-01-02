using MediatR;
using Microsoft.AspNetCore.Mvc;
using TinyDrive.API.Extensions;
using TinyDrive.API.Infrastructure;
using TinyDrive.Application.Abstract;
using TinyDrive.Application.Nodes.GetUploadUrl;

namespace TinyDrive.API.Endpoints.Nodes;

public class GetUploadUrlEndpoint : IEndpoint
{
    private sealed class Request
    {
        public string FileName { get; init; }

        public long FileSize { get; init; }

        public string ContentType { get; init; }
    }

    public void MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapPost("api/nodes/upload-url",
            async ([FromBody] Request request, ISender sender, CancellationToken cancellationToken) =>
            {
                var command = new GetUploadUrlCommand(
                    request.FileName,
                    request.FileSize,
                    request.ContentType
                );

                Result<UploadUrlResponse> result = await sender.Send(command, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);
            });
    }
}
