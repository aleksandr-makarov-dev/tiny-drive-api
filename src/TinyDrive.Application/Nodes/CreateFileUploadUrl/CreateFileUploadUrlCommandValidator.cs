using FluentValidation;

namespace TinyDrive.Application.Nodes.CreateFileUploadUrl;

internal sealed class CreateFileUploadUrlCommandValidator : AbstractValidator<CreateFileUploadUrlCommand>
{
    private const long MaxFileSize = 100 * 1024 * 1024;

    public CreateFileUploadUrlCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(255)
            .Matches(@"^[^\\/:\*\?""<>|]+$")
            .WithMessage("File name contains invalid characters.");

        RuleFor(x => x.Size)
            .GreaterThan(0)
            .LessThanOrEqualTo(MaxFileSize);

        RuleFor(x => x.ContentType)
            .NotEmpty()
            .MaximumLength(100);
    }
}
