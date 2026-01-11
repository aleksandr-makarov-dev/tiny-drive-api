using TinyDrive.Domain.Abstract;

namespace TinyDrive.Application.Abstract.Storage;

public static class ObjectStorageErrors
{
    public static Error UploadUrlCreationFailed() =>
        Error.Failure(
            "upload.url_creation_failed",
            "Failed to create an upload URL.");

    public static Error UploadMetadataNotFound(Ulid fileId) =>
        Error.Conflict(
            "upload.metadata_not_found",
            $"Upload metadata for file with id '{fileId}' was not found.");
    
    public static Error SizeMismatch(long expected, long actual) =>
        Error.Conflict(
            "upload.size_mismatch",
            $"Uploaded file size does not match the expected size. Expected {expected} bytes, got {actual} bytes.");
    
    public static Error ContentTypeMismatch(string expected, string actual) =>
        Error.Conflict(
            "upload.content_type_mismatch",
            $"Uploaded file content type does not match the expected type. Expected '{expected}', got '{actual}'.");
    
    public static Error InvalidUploadTime(Ulid fileId) =>
        Error.Conflict(
            "upload.invalid_timestamp",
            $"The uploaded file has an invalid modification time for file with ID '{fileId}'.");
}
