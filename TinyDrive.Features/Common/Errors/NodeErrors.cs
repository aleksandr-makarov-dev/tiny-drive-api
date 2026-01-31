using ErrorOr;

namespace TinyDrive.Features.Common.Errors;

public static class NodeErrors
{
	private const string Prefix = "Node";

	public static Error ParentFolderNotFound(Guid parentId) =>
		Error.NotFound($"{Prefix}.ParentFolderNotFound", $"Parent folder with ID '{parentId}' was not found.");

	public static Error FolderAlreadyExists(string folderName) =>
		Error.Conflict($"{Prefix}.FolderAlreadyExists", $"Folder '{folderName}' already exists.");

	public static Error FileNotFound(Guid fileId) =>
		Error.NotFound($"{Prefix}.FileNotFound", $"File with ID '{fileId}' was not found.");

	public static Error FileAlreadyExists(string fileName) =>
		Error.Conflict($"{Prefix}.FileAlreadyExists", $"File '{fileName}' already exists.");

	public static Error CreateUploadUrlFailed() =>
		Error.Unexpected($"{Prefix}.CreateUploadUrlFailed", "Failed to create upload URL due to an unexpected error.");

	public static Error GetUploadedObjectFailed() =>
		Error.Unexpected($"{Prefix}.GetUploadedObjectFailed", "Failed to retrieve uploaded object from storage.");

	public static Error FileNotFullyUploaded() =>
		Error.Validation($"{Prefix}.FileNotFullyUploaded", "The file upload has not completed successfully.");

	public static Error CannotConfirmFailedUpload() =>
		Error.Validation($"{Prefix}.CannotConfirmFailedUpload",
			"The file cannot be confirmed because its upload previously failed.");

}
