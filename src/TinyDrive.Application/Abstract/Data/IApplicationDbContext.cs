using Microsoft.EntityFrameworkCore;
using TinyDrive.Domain.Nodes;

namespace TinyDrive.Application.Abstract.Data;

public interface IApplicationDbContext
{
    DbSet<Node> Nodes { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
