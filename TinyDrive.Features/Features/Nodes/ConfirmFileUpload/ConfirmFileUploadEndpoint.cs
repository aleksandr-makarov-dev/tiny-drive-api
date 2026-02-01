using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TinyDrive.Features.Common.Constants;
using TinyDrive.Features.Common.Extensions;

namespace TinyDrive.Features.Features.Nodes.ConfirmFileUpload;

public sealed class ConfirmFileUploadEndpoint : ICarterModule
{
	public void AddRoutes(IEndpointRouteBuilder app)
	{
		app.MapPost("api/nodes/{fileId:guid}/confirm-upload", HandleAsync)
			.WithTags(Tags.Nodes);
	}

	private static async Task<IResult> HandleAsync([FromRoute] Guid fileId, ISender sender)
	{
		var command = new ConfirmFileUploadCommand(fileId);

		var result = await sender.Send(command);

		return result.Match(_ => Results.NoContent(), errors => errors.ToProblem());
	}
}
