using TinyDrive.Domain.Abstract;

namespace TinyDrive.Domain.Nodes;

public static class NodeErrors
{
    public static Error NotFound(Ulid nodeId) =>
        Error.NotFound(
            "nodes.not_found",
            $"The item with id '{nodeId}' was not found.");

    public static Error ParentNotFound(Ulid parentId) =>
        Error.NotFound(
            "nodes.parent_not_found",
            $"The parent folder with id '{parentId}' was not found.");

    public static Error MustBeFile(Ulid nodeId) =>
        Error.Conflict(
            "nodes.not_a_file",
            $"The item with id '{nodeId}' is not a file.");

    public static Error MustBeFolder(Ulid nodeId) =>
        Error.Conflict(
            "nodes.not_a_folder",
            $"The item with id '{nodeId}' is not a folder.");
    
    public static Error ParentMustBeFolder(Ulid nodeId) =>
        Error.Conflict(
            "nodes.parent_not_a_folder",
            $"The parent with id '{nodeId}' is not a folder.");

    public static Error Duplicate(string name, Ulid? parentId) =>
        Error.Conflict(
            "nodes.duplicate",
            $"An item named '{name}' already exists in the folder with id '{parentId}'.");
}
