using MediatR;
using Microsoft.AspNetCore.Mvc;
using TinyDrive.API.Extensions;
using TinyDrive.API.Infrastructure;
using TinyDrive.Application.Abstract;
using TinyDrive.Application.Nodes.GetFolderItems;

namespace TinyDrive.API.Endpoints.Nodes;

public class GetFolderItemsEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapGet("api/nodes", async ([FromQuery] Ulid? parentId, ISender sender) =>
        {
            var query = new GetFolderItemsQuery(parentId);

            Result<PagedResult<FolderItemResponse>> result = await sender.Send(query);

            return result.Match(Results.Ok, CustomResults.Problem);
        });
    }
}
