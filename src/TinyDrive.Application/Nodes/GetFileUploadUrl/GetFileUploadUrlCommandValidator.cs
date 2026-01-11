using FluentValidation;

namespace TinyDrive.Application.Nodes.GetFileUploadUrl;

internal sealed class GetFileUploadUrlCommandValidator : AbstractValidator<GetFileUploadUrlCommand>
{
    private const long MaxFileSize = 100 * 1024 * 1024;

    public GetFileUploadUrlCommandValidator()
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
