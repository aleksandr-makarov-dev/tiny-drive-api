using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TinyDrive.Features.Common.Constants;
using TinyDrive.Features.Common.Extensions;

namespace TinyDrive.Features.Features.Nodes.CreateFileUploadUrl;

public sealed class CreateFileUploadUrlEndpoint : ICarterModule
{

	public void AddRoutes(IEndpointRouteBuilder app)
	{
		app.MapPost("api/nodes/upload-url", HandleAsync)
			.WithTags(Tags.Nodes);
	}

	private static async Task<IResult> HandleAsync([FromBody] CreateFileUploadUrlRequest request, ISender sender)
	{
		var command = new CreateFileUploadUrlCommand(request.FileName, request.FileSizeBytes, request.ContentType,
			request.ParentFolderId);

		var result = await sender.Send(command);

		return result.Match(Results.Ok, errors => errors.ToProblem());
	}
}
