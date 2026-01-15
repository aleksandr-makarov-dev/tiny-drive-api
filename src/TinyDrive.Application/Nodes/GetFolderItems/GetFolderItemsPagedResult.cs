using TinyDrive.Application.Abstract;

namespace TinyDrive.Application.Nodes.GetFolderItems;

public sealed record GetFolderItemsPagedResult(
    IEnumerable<FolderItemResponse> Items,
    long? PaginationToken = null,
    Ulid? ParentFolderId = null) : PagedResult<FolderItemResponse>(Items, PaginationToken);
