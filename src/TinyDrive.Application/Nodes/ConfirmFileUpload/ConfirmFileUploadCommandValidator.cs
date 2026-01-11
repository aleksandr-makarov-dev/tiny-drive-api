using FluentValidation;

namespace TinyDrive.Application.Nodes.ConfirmFileUpload;

internal sealed class ConfirmFileUploadCommandValidator : AbstractValidator<ConfirmFileUploadCommand>
{
    public ConfirmFileUploadCommandValidator()
    {
        RuleFor(x => x.FileId).NotNull();
    }
}
