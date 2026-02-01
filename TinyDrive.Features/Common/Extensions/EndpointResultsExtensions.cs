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

		var title = errors[0].Type switch
		{
			ErrorType.Conflict => "Conflict: the request could not be completed.",
			ErrorType.Validation => "One or more validation errors occurred.",
			ErrorType.NotFound => "Requested resource(s) not found.",
			ErrorType.Unauthorized => "Unauthorized: you do not have permission.",
			_ => "An unknown error has occurred."
		};

		return Results.ValidationProblem(title: title,
			errors: errors.ToDictionary(k => k.Code,
				v =>
					new[]
					{
						v.Description
					}, StringComparer.Ordinal),
			statusCode: statusCode);
	}
}
