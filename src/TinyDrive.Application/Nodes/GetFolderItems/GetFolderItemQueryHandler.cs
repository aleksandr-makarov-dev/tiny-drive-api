using MediatR;
using Microsoft.Extensions.Logging;
using TinyDrive.Application.Abstract;
using TinyDrive.Application.Abstract.Data.Repositories;
using TinyDrive.Domain.Nodes;

namespace TinyDrive.Application.Nodes.GetFolderItems;

internal sealed class GetFolderItemQueryHandler(
    INodeRepository nodeRepository,
    ILogger<GetFolderItemQueryHandler> logger
)
    : IRequestHandler<GetFolderItemsQuery, Result<GetFolderItemsPagedResult>>
{
    public async Task<Result<GetFolderItemsPagedResult>> Handle(GetFolderItemsQuery request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Retrieving folder items for {RequestParentId} folder.", request.ParentId);

        Ulid? parentFolderId = null;

        if (request.ParentId is not null)
        {
            Node? parent = await nodeRepository.FindByIdAsync(request.ParentId.Value, cancellationToken);

            if (parent is null)
            {
                logger.LogWarning("Parent node not found.");

                return Result.Failure<GetFolderItemsPagedResult>(
                    NodeErrors.ParentNotFound(request.ParentId.Value));
            }

            if (!parent.IsFolder)
            {
                logger.LogWarning("Parent is not a folder.");

                return Result.Failure<GetFolderItemsPagedResult>(
                    NodeErrors.ParentMustBeFolder(request.ParentId.Value));
            }

            parentFolderId = parent.ParentId;
        }

        List<Node> items =
            await nodeRepository.FindAllByParentAsync(request.ParentId, cancellationToken: cancellationToken);

        var pageResult = new GetFolderItemsPagedResult(items.Select(MapToFolderItemResponse).ToList(),
            ParentFolderId: parentFolderId);

        return Result.Success(pageResult);
    }

    private static FolderItemResponse MapToFolderItemResponse(Node item)
    {
        return new FolderItemResponse
        {
            Id = item.Id,
            Name = item.DisplayName,
            ParentId = item.ParentId,
            IsFolder = item.IsFolder,
            ContentType = item.ContentType,
            Size = item.Size,
            CreatedAtUtc = item.CreatedAtUtc,
            LastModifiedAtUtc = item.LastModifiedAtUtc,
        };
    }
}
