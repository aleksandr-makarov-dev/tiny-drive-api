using Carter;
using MediatR;
using TinyDrive.Features.Common.Constants;
using TinyDrive.Features.Common.Extensions;

namespace TinyDrive.Features.Features.Nodes.EmptyTrash;

public sealed class EmptyTrashEndpoint : ICarterModule
{

	public void AddRoutes(IEndpointRouteBuilder app)
	{
		app.MapDelete("api/nodes/trash", HandleAsync)
			.WithTags(Tags.Nodes);
	}

	private static async Task<IResult> HandleAsync(ISender sender)
	{
		var command = new EmptyTrashCommand();

		var result = await sender.Send(command);

		return result.Match(_ => Results.NoContent(), errors => errors.ToProblem());
	}
}
