namespace TinyDrive.Features.Features.Nodes.GetFolderItems;

public record FolderItem(Guid Id, string Name, bool IsFolder, DateTime CreatedAtUtc, DateTime? LastModifiedAtUtc);
