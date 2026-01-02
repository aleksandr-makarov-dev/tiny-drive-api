using MediatR;
using Microsoft.AspNetCore.Mvc;
using TinyDrive.API.Extensions;
using TinyDrive.API.Infrastructure;
using TinyDrive.Application.Abstract;
using TinyDrive.Application.Nodes.ConfirmUpload;

namespace TinyDrive.API.Endpoints.Nodes;

public class ConfirmUploadEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapPost("api/nodes/{id:guid}/confirm-upload", async ([FromRoute] Guid id, ISender sender) =>
        {
            var command = new ConfirmUploadCommand(id);

            Result result = await sender.Send(command);

            return result.Match(Results.NoContent, CustomResults.Problem);
        });
    }
}
