using Microsoft.EntityFrameworkCore;
using TinyDrive.Application.Abstract.Data.Repositories;
using TinyDrive.Domain.Nodes;
using TinyDrive.Infrastructure.Data;

namespace TinyDrive.Infrastructure.Repositories;

internal sealed class NodeRepository(ApplicationDbContext dbContext) : INodeRepository
{
    public void Add(Node node)
    {
        dbContext.Nodes.Add(node);
    }

    /// <summary>
    /// Finds a node by Id without tracking.
    /// </summary>
    /// <param name="id">Node Id.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The node or null if not found.</returns>
    public Task<Node?> FindByIdAsync(Ulid id, CancellationToken cancellationToken = default)
    {
        return dbContext.Nodes
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken: cancellationToken);
    }

    public Task<Node?> FindByAsync(string name, string? extension = null, Ulid? parentId = null,
        CancellationToken cancellationToken = default)
    {
        return dbContext.Nodes
            .AsNoTracking()
            .FirstOrDefaultAsync(x =>
                    x.Name == name
                    && x.Extension == extension
                    && x.ParentId == parentId,
                cancellationToken: cancellationToken);
    }

    public Task<List<Node>> FindAllByParentAsync(Ulid? parentId = null,
        CancellationToken cancellationToken = default)
    {
        return dbContext.Nodes
            .AsNoTracking()
            .Where(x => x.ParentId == parentId)
            .OrderBy(x => x.IsFolder)
            .ThenBy(x => x.Id)
            .ToListAsync(cancellationToken: cancellationToken);
    }

    public Task<bool> ExistsAsync(string name, string? extension = null, Ulid? parentId = null,
        CancellationToken cancellationToken = default)
    {
        return dbContext.Nodes.AnyAsync(x =>
                x.Name == name
                && x.Extension == extension
                && x.ParentId == parentId,
            cancellationToken: cancellationToken);
    }

    public Task<bool> ExistsByIdAsync(Ulid id, CancellationToken cancellationToken = default)
    {
        return dbContext.Nodes.AnyAsync(x => x.Id == id, cancellationToken);
    }
}
