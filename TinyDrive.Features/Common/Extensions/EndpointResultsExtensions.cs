using ErrorOr;

namespace TinyDrive.Features.Common.Extensions;

internal static class EndpointResultsExtensions
{
	public static IResult ToProblem(this List<Error> errors)
	{
		return errors.Count is 0 ? Results.Problem() : CreateProblem(errors);
	}

	private static IResult CreateProblem(List<Error> errors)
	{
		var statusCode = errors[0].Type switch
		{
			ErrorType.Conflict => StatusCodes.Status409Conflict,
			ErrorType.Validation => StatusCodes.Status400BadRequest,
			ErrorType.NotFound => StatusCodes.Status404NotFound,
			ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
			_ => StatusCodes.Status500InternalServerError
		};

		return Results.ValidationProblem(errors.ToDictionary(k => k.Code, v => new[]
			{
				v.Description
			}, StringComparer.Ordinal),
			statusCode: statusCode);
	}
}
