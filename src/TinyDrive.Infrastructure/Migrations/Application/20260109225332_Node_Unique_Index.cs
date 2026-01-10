using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TinyDrive.Infrastructure.Migrations.Application;

/// <inheritdoc />
public partial class Node_Unique_Index : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "ix_nodes_parent_id",
            schema: "public",
            table: "nodes");

        migrationBuilder.CreateIndex(
            name: "ix_nodes_parent_id_name_extension",
            schema: "public",
            table: "nodes",
            columns: new[] { "parent_id", "name", "extension" },
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "ix_nodes_parent_id_name_extension",
            schema: "public",
            table: "nodes");

        migrationBuilder.CreateIndex(
            name: "ix_nodes_parent_id",
            schema: "public",
            table: "nodes",
            column: "parent_id");
    }
}
