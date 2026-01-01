using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TinyDrive.Domain.Nodes;

namespace TinyDrive.Infrastructure.Data.Configurations;

internal sealed class NodeConfiguration : IEntityTypeConfiguration<Node>
{
    public void Configure(EntityTypeBuilder<Node> builder)
    {
        builder.ToTable("nodes");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(255);

        builder.HasDiscriminator<NodeType>("Type")
            .HasValue<FileNode>(NodeType.File)
            .HasValue<FolderNode>(NodeType.Folder);
    }
}