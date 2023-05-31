using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MySuperApi.Migrations
{
    /// <inheritdoc />
    public partial class superUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "CurrentImageBytes",
                table: "Users",
                type: "bytea",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentImageBytes",
                table: "Users");
        }
    }
}
