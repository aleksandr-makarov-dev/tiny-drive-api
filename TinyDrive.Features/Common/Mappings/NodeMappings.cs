using TinyDrive.Domain.Nodes;
using TinyDrive.Features.Features.Nodes.GetFolderItems;

namespace TinyDrive.Features.Common.Mappings;

public static class NodeMappings
{
	public static IQueryable<FolderItem> ProjectToFolderItems(this IQueryable<Node> query)
	{
		return query.Select(x => new FolderItem(x.Id, x.DisplayName, x.IsFolder, x.CreatedAtUtc, x.LastModifiedAtUtc));
	}
}
