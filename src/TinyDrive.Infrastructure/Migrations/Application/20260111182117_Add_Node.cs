using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TinyDrive.Infrastructure.Migrations.Application;

/// <inheritdoc />
public partial class Add_Node : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.EnsureSchema(
            name: "public");

        migrationBuilder.CreateTable(
            name: "nodes",
            schema: "public",
            columns: table => new
            {
                id = table.Column<byte[]>(type: "bytea", nullable: false),
                name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                extension = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                content_type = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                size = table.Column<long>(type: "bigint", nullable: false),
                parent_id = table.Column<byte[]>(type: "bytea", nullable: true),
                is_folder = table.Column<bool>(type: "boolean", nullable: false),
                upload_status = table.Column<int>(type: "integer", nullable: true),
                created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                last_modified_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("pk_nodes", x => x.id);
                table.ForeignKey(
                    name: "fk_nodes_nodes_parent_id",
                    column: x => x.parent_id,
                    principalSchema: "public",
                    principalTable: "nodes",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Restrict);
            });

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
        migrationBuilder.DropTable(
            name: "nodes",
            schema: "public");
    }
}
