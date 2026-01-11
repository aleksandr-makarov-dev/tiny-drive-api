using MediatR;
using Microsoft.AspNetCore.Mvc;
using TinyDrive.API.Extensions;
using TinyDrive.API.Infrastructure;
using TinyDrive.Application.Nodes.CreateFolder;
using TinyDrive.SharedKernel;

namespace TinyDrive.API.Endpoints.Nodes;

internal sealed class CreateFolderEndpoint : IEndpoint
{
    private sealed class CreateFolderRequest
    {
        public string Name { get; init; }

        public Ulid? ParentId { get; init; }
    }

    public void MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapPost("api/nodes/create-folder", async ([FromBody] CreateFolderRequest request, ISender sender) =>
        {
            var command = new CreateFolderCommand(request.Name, request.ParentId);

            Result<Ulid> result = await sender.Send(command);

            return result.Match(Results.Ok, CustomResults.Problem);
        });
    }
}
