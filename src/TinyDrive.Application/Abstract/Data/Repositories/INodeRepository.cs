using TinyDrive.Domain.Nodes;

namespace TinyDrive.Application.Abstract.Data.Repositories;

public interface INodeRepository
{
    void Add(Node node);

    Task<Node?> FindByIdAsync(Ulid id, CancellationToken cancellationToken = default);

    Task<Node?> FindByAsync(string name, string? extension = null, Ulid? parentId = null,
        CancellationToken cancellationToken = default);

    Task<List<Node>> FindAllByParentAsync(Ulid? parentId = null, CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(string name, string? extension = null, Ulid? parentId = null,
        CancellationToken cancellationToken = default);
    
    Task<bool> ExistsByIdAsync(Ulid id, CancellationToken cancellationToken = default);
}
