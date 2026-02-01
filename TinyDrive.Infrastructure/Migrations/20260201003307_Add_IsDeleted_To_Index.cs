using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TinyDrive.Infrastructure.Migrations;

    /// <inheritdoc />
    [SuppressMessage("Performance", "CA1861:Avoid constant arrays as arguments")]
    public partial class Add_IsDeleted_To_Index : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_nodes_parent_id_name_extension_is_folder",
                table: "nodes");

            migrationBuilder.CreateIndex(
                name: "ix_nodes_parent_id_name_extension_is_folder_is_deleted",
                table: "nodes",
                columns: new[] { "parent_id", "name", "extension", "is_folder", "is_deleted" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_nodes_parent_id_name_extension_is_folder_is_deleted",
                table: "nodes");

            migrationBuilder.CreateIndex(
                name: "ix_nodes_parent_id_name_extension_is_folder",
                table: "nodes",
                columns: new[] { "parent_id", "name", "extension", "is_folder" },
                unique: true);
        }
    }
