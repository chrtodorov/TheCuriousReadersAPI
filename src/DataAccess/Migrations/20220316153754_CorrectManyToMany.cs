using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class CorrectManyToMany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookEntityUserEntity");

            migrationBuilder.CreateTable(
                name: "UserBooksEntity",
                columns: table => new
                {
                    UserBooksId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BookId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBooksEntity", x => x.UserBooksId);
                    table.ForeignKey(
                        name: "FK_UserBooksEntity_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "BookId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserBooksEntity_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserBooksEntity_BookId",
                table: "UserBooksEntity",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_UserBooksEntity_UserId",
                table: "UserBooksEntity",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserBooksEntity");

            migrationBuilder.CreateTable(
                name: "BookEntityUserEntity",
                columns: table => new
                {
                    BooksBookId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsersUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookEntityUserEntity", x => new { x.BooksBookId, x.UsersUserId });
                    table.ForeignKey(
                        name: "FK_BookEntityUserEntity_Books_BooksBookId",
                        column: x => x.BooksBookId,
                        principalTable: "Books",
                        principalColumn: "BookId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookEntityUserEntity_Users_UsersUserId",
                        column: x => x.UsersUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookEntityUserEntity_UsersUserId",
                table: "BookEntityUserEntity",
                column: "UsersUserId");
        }
    }
}
