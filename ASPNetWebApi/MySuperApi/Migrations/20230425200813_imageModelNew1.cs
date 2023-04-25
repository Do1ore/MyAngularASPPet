using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MySuperApi.Migrations
{
    /// <inheritdoc />
    public partial class imageModelNew1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApiImages",
                columns: table => new
                {
                    ImageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiImages", x => x.ImageId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApiImages");
        }
    }
}
