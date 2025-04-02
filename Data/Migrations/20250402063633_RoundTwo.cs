using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewsSite.Data.Migrations
{
    /// <inheritdoc />
    public partial class RoundTwo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CustomUsername",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomUsername",
                table: "AspNetUsers");
        }
    }
}
