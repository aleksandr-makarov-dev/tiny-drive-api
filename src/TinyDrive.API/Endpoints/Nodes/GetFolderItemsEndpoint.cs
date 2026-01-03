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
        builder.MapGet("api/nodes/{id:guid}/items",
            async ([FromRoute] Guid id, ISender sender, CancellationToken cancellationToken) =>
            {
                var query = new GetFolderItemsQuery(id);

                Result<PagedResult<FolderItemResponse>> result = await sender.Send(query, cancellationToken);

                return result.Match(Results.Ok, CustomResults.Problem);
            });
    }
}
