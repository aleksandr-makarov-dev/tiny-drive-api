using MediatR;
using Microsoft.Extensions.Logging;
using TinyDrive.Application.Abstract.Data;
using TinyDrive.Application.Abstract.Data.Repositories;
using TinyDrive.Domain.Nodes;
using TinyDrive.SharedKernel;

namespace TinyDrive.Application.Nodes.CreateFolder;

internal sealed class CreateFolderCommandHandler(
    IUnitOfWork unitOfWork,
    INodeRepository nodeRepository,
    IDateTimeProvider dateTimeProvider,
    ILogger<CreateFolderCommandHandler> logger)
    : IRequestHandler<CreateFolderCommand, Result<Ulid>>
{
    public async Task<Result<Ulid>> Handle(CreateFolderCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating folder {FolderName}.", request.Name);

        if (request.ParentId is not null)
        {
            Node? parent = await nodeRepository.FindByIdAsync(request.ParentId.Value, cancellationToken);

            if (parent is null)
            {
                logger.LogWarning("Parent node not found.");

                return Result.Failure<Ulid>(Error.NotFound("Nodes.NotFound",
                    $"The parent with id = {request.ParentId} was not found."));
            }

            if (!parent.IsFolder)
            {
                logger.LogWarning("Parent is not a folder.");

                return Result.Failure<Ulid>(Error.Conflict("Nodes.ParentMustBeFolder",
                    $"The parent with id = {request.ParentId} is not a folder."));
            }
        }

        bool isDuplicate = await nodeRepository.ExistsAsync(request.Name, null, request.ParentId, cancellationToken);

        if (isDuplicate)
        {
            logger.LogWarning("Duplicate folder {FolderName}.", request.Name);

            return Result.Failure<Ulid>(Error.Conflict("Nodes.Duplicate",
                $"The folder with name = {request.Name} already exists in parent folder."));
        }

        var node = Node.NewFolder(request.Name, dateTimeProvider.UtcNow, request.ParentId);

        nodeRepository.Add(node);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(node.Id);
    }
}
