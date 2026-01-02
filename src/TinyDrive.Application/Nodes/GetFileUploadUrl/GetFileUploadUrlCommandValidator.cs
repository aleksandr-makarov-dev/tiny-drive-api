using FluentValidation;

namespace TinyDrive.Application.Nodes.GetFileUploadUrl;

public class GetFileUploadUrlCommandValidator : AbstractValidator<GetFileUploadUrlCommand>
{
    public GetFileUploadUrlCommandValidator()
    {
        RuleFor(x => x.FileName)
            .NotEmpty()
            .MaximumLength(255);

        RuleFor(x => x.FileSize)
            .GreaterThan(0)
            .LessThanOrEqualTo(100 * 1024 * 1024); // TODO: move max file size to appsettings.json

        RuleFor(x => x.ContentType).NotEmpty()
            .MaximumLength(100);


        // private static readonly string[] AllowedMimeTypes = 
        // {
        //     "application/pdf",
        //     "image/png",
        //     "image/jpeg"
        // };
        //
        // RuleFor(x => x.ContentType)
        //     .Must(ct => AllowedMimeTypes.Contains(ct))
        //     .WithMessage("Unsupported content type");

    }
}
