namespace TinyDrive.Application.Abstract;

public record PagedResult<T>(IEnumerable<T> Items, long? PaginationToken = null);
