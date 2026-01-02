using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TinyDrive.Infrastructure.Migrations.Application;

/// <inheritdoc />
public partial class Add_Node_UploadStatus : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<int>(
            name: "upload_status",
            schema: "public",
            table: "nodes",
            type: "integer",
            nullable: false,
            defaultValue: 0);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "upload_status",
            schema: "public",
            table: "nodes");
    }
}
