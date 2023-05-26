using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MySuperApi.Migrations
{
    /// <inheritdoc />
    public partial class chatLastMessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Lastmessage",
                table: "Chats",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Lastmessage",
                table: "Chats");
        }
    }
}
