using MediatR;
using Microsoft.AspNetCore.Mvc;
using TinyDrive.API.Extensions;
using TinyDrive.API.Infrastructure;
using TinyDrive.Application.Abstract;
using TinyDrive.Application.Nodes.ConfirmFileUpload;

namespace TinyDrive.API.Endpoints.Nodes;

internal sealed class ConfirmFileUploadEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapPost("api/nodes/{fileId}/confirm-upload", async ([FromRoute] Ulid fileId, ISender sender) =>
        {
            var command = new ConfirmFileUploadCommand(fileId);

            Result result = await sender.Send(command);

            return result.Match(Results.NoContent, CustomResults.Problem);
        });
    }
}
