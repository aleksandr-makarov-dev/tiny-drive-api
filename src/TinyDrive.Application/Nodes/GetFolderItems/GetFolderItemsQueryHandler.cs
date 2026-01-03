using MediatR;
using Microsoft.EntityFrameworkCore;
using TinyDrive.Application.Abstract;
using TinyDrive.Application.Abstract.Data;
using TinyDrive.Domain.Nodes;


namespace TinyDrive.Application.Nodes.GetFolderItems;

internal sealed class GetFolderItemsQueryHandler(IApplicationDbContext dbContext)
    : IRequestHandler<GetFolderItemsQuery, Result<PagedResult<FolderItemResponse>>>
{
    public async Task<Result<PagedResult<FolderItemResponse>>> Handle(GetFolderItemsQuery request,
        CancellationToken cancellationToken)
    {
        List<FolderItemResponse> folderItems = await dbContext
            .Nodes
            .AsNoTracking()
            .Where(x => x.UploadStatus == NodeUploadStatus.Uploaded)
            .Select(x => new FolderItemResponse
            {
                Id = x.Id,
                Name = x.Name,
                Type = x.Type,
                Size = x.Size,
                ContentType = x.ContentType,
                CreatedAtUtc = x.CreatedAtUtc,
            })
            .ToListAsync(cancellationToken);

        var pagedResult = PagedResult<FolderItemResponse>.Of(folderItems, null);

        return Result.Success(pagedResult);
    }
}
