using FluentValidation;

namespace TinyDrive.Features.Features.Nodes.CreateFileUploadUrl;

public sealed class CreateFileUploadUrlCommandValidator : AbstractValidator<CreateFileUploadUrlCommand>
{
	private const long MaxFileSizeBytes = 100 * 1024 * 1024;

	public CreateFileUploadUrlCommandValidator()
	{
		RuleFor(x => x.FileName)
			.NotEmpty()
			.MaximumLength(255)
			.Matches(@"^(?!.*\.\.)[A-Za-z0-9._-]+\.[A-Za-z0-9]+$");

		RuleFor(x => x.FileSizeBytes)
			.GreaterThan(0)
			.LessThanOrEqualTo(MaxFileSizeBytes);

		RuleFor(x => x.ContentType)
			.NotEmpty()
			.MaximumLength(100);
	}
}
