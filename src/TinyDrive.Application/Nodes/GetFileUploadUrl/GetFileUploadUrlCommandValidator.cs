using FluentValidation;

namespace TinyDrive.Application.Nodes.GetFileUploadUrl;

public class GetFileUploadUrlCommandValidator : AbstractValidator<GetFileUploadUrlCommand>
{
    public GetFileUploadUrlCommandValidator()
    {
        RuleFor(x => x.FileName)
            .NotEmpty()
            .MaximumLength(255);

        RuleFor(x => x.FileSize).GreaterThan(0);

        RuleFor(x => x.ContentType).NotEmpty()
            .MaximumLength(100);
    }
}
