using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MySuperApi.Migrations
{
    /// <inheritdoc />
    public partial class chat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProfileImageStorages_UserProfileImages_ProfileImageId",
                table: "ProfileImageStorages");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProfileImages_Users_AppUserId",
                table: "UserProfileImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserProfileImages",
                table: "UserProfileImages");

            migrationBuilder.RenameTable(
                name: "UserProfileImages",
                newName: "ProfileImages");

            migrationBuilder.RenameIndex(
                name: "IX_UserProfileImages_AppUserId",
                table: "ProfileImages",
                newName: "IX_ProfileImages_AppUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProfileImages",
                table: "ProfileImages",
                column: "ImageId");

            migrationBuilder.CreateTable(
                name: "Chats",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chats", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: true),
                    SentAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SenderId = table.Column<Guid>(type: "uuid", nullable: false),
                    ChatId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProfileImageClaims",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProfileImageId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileImageClaims", x => new { x.UserId, x.ProfileImageId });
                    table.ForeignKey(
                        name: "FK_ProfileImageClaims_ProfileImages_ProfileImageId",
                        column: x => x.ProfileImageId,
                        principalTable: "ProfileImages",
                        principalColumn: "ImageId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProfileImageClaims_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChatUsers",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ChatId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatUsers", x => new { x.UserId, x.ChatId });
                    table.ForeignKey(
                        name: "FK_ChatUsers_Chats_ChatId",
                        column: x => x.ChatId,
                        principalTable: "Chats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChatUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChatUsers_ChatId",
                table: "ChatUsers",
                column: "ChatId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileImageClaims_ProfileImageId",
                table: "ProfileImageClaims",
                column: "ProfileImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProfileImages_Users_AppUserId",
                table: "ProfileImages",
                column: "AppUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProfileImageStorages_ProfileImages_ProfileImageId",
                table: "ProfileImageStorages",
                column: "ProfileImageId",
                principalTable: "ProfileImages",
                principalColumn: "ImageId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProfileImages_Users_AppUserId",
                table: "ProfileImages");

            migrationBuilder.DropForeignKey(
                name: "FK_ProfileImageStorages_ProfileImages_ProfileImageId",
                table: "ProfileImageStorages");

            migrationBuilder.DropTable(
                name: "ChatUsers");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "ProfileImageClaims");

            migrationBuilder.DropTable(
                name: "Chats");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProfileImages",
                table: "ProfileImages");

            migrationBuilder.RenameTable(
                name: "ProfileImages",
                newName: "UserProfileImages");

            migrationBuilder.RenameIndex(
                name: "IX_ProfileImages_AppUserId",
                table: "UserProfileImages",
                newName: "IX_UserProfileImages_AppUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserProfileImages",
                table: "UserProfileImages",
                column: "ImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProfileImageStorages_UserProfileImages_ProfileImageId",
                table: "ProfileImageStorages",
                column: "ProfileImageId",
                principalTable: "UserProfileImages",
                principalColumn: "ImageId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfileImages_Users_AppUserId",
                table: "UserProfileImages",
                column: "AppUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
