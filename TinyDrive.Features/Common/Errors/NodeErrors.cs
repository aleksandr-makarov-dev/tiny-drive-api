using ErrorOr;

namespace TinyDrive.Features.Common.Errors;

public static class NodeErrors
{
	private const string Prefix = "Node";

	public static Error ParentFolderNotFound() =>
		Error.NotFound(
			$"{Prefix}.ParentFolderNotFound",
			"Parent folder not found.");

	public static Error FolderAlreadyExists() =>
		Error.Conflict(
			$"{Prefix}.FolderAlreadyExists",
			"Folder already exists.");

	public static Error NodeNotFound(Guid nodeId) =>
		Error.NotFound(
			$"{Prefix}.NotFound",
			$"Node with id '{nodeId}' was not found.");
}
