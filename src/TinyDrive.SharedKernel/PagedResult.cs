namespace TinyDrive.SharedKernel;

public record PagedResult<T>(IEnumerable<T> Items, long? PaginationToken = null);
