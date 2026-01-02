using FluentValidation;

namespace TinyDrive.Application.Nodes.ConfirmUpload;

public class ConfirmUploadValidator : AbstractValidator<ConfirmUploadCommand>
{
    public ConfirmUploadValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
