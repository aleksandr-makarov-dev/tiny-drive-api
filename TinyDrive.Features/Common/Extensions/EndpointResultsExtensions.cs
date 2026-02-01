using ErrorOr;

namespace TinyDrive.Features.Common.Extensions;

internal static class EndpointResultsExtensions
{
	public static IResult ToProblem(this List<Error> errorList)
	{
		return errorList.Count == 0 ? Results.Problem() : MapErrorsToHttpResult(errorList);
	}

	private static IResult MapErrorsToHttpResult(List<Error> errorList)
	{
		var firstError = errorList[0];
		var errorType = firstError.Type;

		var statusCode = errorType switch
		{
			ErrorType.Conflict => StatusCodes.Status409Conflict,
			ErrorType.Validation => StatusCodes.Status400BadRequest,
			ErrorType.NotFound => StatusCodes.Status404NotFound,
			ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
			_ => StatusCodes.Status500InternalServerError
		};

		var title = errorType switch
		{
			ErrorType.Conflict => "Conflict: the request could not be completed.",
			ErrorType.Validation => "One or more validation errors occurred.",
			ErrorType.NotFound => "Requested resource(s) not found.",
			ErrorType.Unauthorized => "Unauthorized: you do not have permission.",
			_ => "An unknown error has occurred."
		};

		return errorType == ErrorType.Validation
			? BuildValidationProblem(title, statusCode, errorList)
			: BuildErrorResponse(title, statusCode, errorList);
	}

	private static IResult BuildValidationProblem(string title, int statusCode, List<Error> errorList)
	{
		return Results.ValidationProblem(
			errors: errorList.ToDictionary(
				k => k.Code,
				v => new[]
				{
					v.Description
				},
				StringComparer.Ordinal),
			statusCode: statusCode,
			title: title);
	}

	private static IResult BuildErrorResponse(string title, int statusCode, List<Error> errorList)
	{
		return Results.Problem(
			detail: errorList[0].Description,
			statusCode: statusCode,
			title: title);
	}
}
