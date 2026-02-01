using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TinyDrive.Features.Common.Constants;
using TinyDrive.Features.Common.Extensions;

namespace TinyDrive.Features.Features.Nodes.GetFolderItems;

public sealed class GetFolderItemsEndpoint : ICarterModule
{

	public void AddRoutes(IEndpointRouteBuilder app)
	{
		app.MapGet("api/nodes", HandleAsync)
			.WithTags(Tags.Nodes);
	}

	private static async Task<IResult> HandleAsync(ISender sender, [FromQuery] Guid? parentId,
		[FromQuery] int pageNumber = 1,
		[FromQuery] int pageSize = 25)
	{
		var query = new GetFolderItemsQuery(parentId, pageNumber, pageSize);

		var result = await sender.Send(query);

		return result.Match(Results.Ok, errors => errors.ToProblem());
	}
}
