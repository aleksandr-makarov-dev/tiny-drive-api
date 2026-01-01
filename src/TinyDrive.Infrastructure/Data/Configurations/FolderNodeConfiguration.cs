using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TinyDrive.Domain.Nodes;

namespace TinyDrive.Infrastructure.Data.Configurations;

internal sealed class FolderNodeConfiguration : IEntityTypeConfiguration<FolderNode>
{
    public void Configure(EntityTypeBuilder<FolderNode> builder)
    {
    }
}