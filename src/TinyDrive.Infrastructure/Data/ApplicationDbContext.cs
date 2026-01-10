using Microsoft.EntityFrameworkCore;
using TinyDrive.Application.Abstract.Data;
using TinyDrive.Domain.Nodes;
using TinyDrive.Infrastructure.Data.Converters;

namespace TinyDrive.Infrastructure.Data;

public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : DbContext(options), IUnitOfWork
{
    public DbSet<Node> Nodes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        modelBuilder.HasDefaultSchema(Schemas.Default);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<Ulid>()
            .HaveConversion<UlidToBytesConverter>();
    }
}
