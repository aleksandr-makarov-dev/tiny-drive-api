using System.ComponentModel.DataAnnotations.Schema;
using TinyDrive.Domain.Common;

namespace TinyDrive.Domain.Nodes;

public sealed class Node : Entity
{
	public required string Name { get; init; }

	public string? Extension { get; init; }

	public long? Size { get; init; }

	public string? ContentType { get; init; }

	public bool IsFolder { get; init; }

	public Guid? ParentId { get; init; }

	public DateTime CreatedAtUtc { get; init; }

	public DateTime? LastModifiedAtUtc { get; init; }

	[NotMapped] public string DisplayName => string.IsNullOrEmpty(Extension) ? Name : $"{Name}.{Extension}";
}
