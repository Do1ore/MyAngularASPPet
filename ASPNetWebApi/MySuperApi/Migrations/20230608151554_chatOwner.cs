using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MySuperApi.Migrations
{
    /// <inheritdoc />
    public partial class chatOwner : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ChatAdministrator",
                table: "Chats",
                type: "uuid",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChatAdministrator",
                table: "Chats");
        }
    }
}
