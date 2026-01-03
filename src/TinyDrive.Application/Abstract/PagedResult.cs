namespace TinyDrive.Application.Abstract;

using System;
using System.Collections.Generic;

public sealed class PagedResult<T>
{
    private PagedResult() { }

    public IEnumerable<T> Items { get; private init; } = [];

    public Guid? PaginationToken { get; private init; }

    public static PagedResult<T> Of(
        IEnumerable<T> items,
        Guid? paginationToken = null)
    {
        ArgumentNullException.ThrowIfNull(items);

        return new PagedResult<T>
        {
            Items = items,
            PaginationToken = paginationToken
        };
    }
}
