using MediatR;
using Microsoft.AspNetCore.Mvc;
using TinyDrive.API.Extensions;
using TinyDrive.API.Infrastructure;
using TinyDrive.Application.Abstract;
using TinyDrive.Application.Nodes.GetFileUploadUrl;

namespace TinyDrive.API.Endpoints.Nodes;

internal sealed class GetFileUploadUrlEndpoint : IEndpoint
{
    private sealed class Request
    {
        public string Name { get; init; }

        public long Size { get; init; }

        public string ContentType { get; init; }

        public Ulid? ParentId { get; init; }
    }

    public void MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapPost("api/nodes/upload-url", async ([FromBody] Request request, ISender sender) =>
        {
            var command = new GetFileUploadUrlCommand(
                request.Name,
                request.Size,
                request.ContentType,
                request.ParentId
            );

            Result<FileUploadUrlResponse> response = await sender.Send(command);

            return response.Match(Results.Ok, CustomResults.Problem);
        });
    }
}
