using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TinyDrive.Domain.Nodes;

namespace TinyDrive.Infrastructure.Data.Configurations;

internal sealed class FileNodeConfiguration : IEntityTypeConfiguration<FileNode>
{
    public void Configure(EntityTypeBuilder<FileNode> builder)
    {
        builder.Property(x => x.ContentType)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Size)
            .IsRequired();
    }
}