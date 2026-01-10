using FluentValidation;

namespace TinyDrive.Application.Nodes.CreateFolder;

internal sealed class CreateFolderCommandValidator : AbstractValidator<CreateFolderCommand>
{
    public CreateFolderCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(255);
    }
}
