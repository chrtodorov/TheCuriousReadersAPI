using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class AddAuthorsToDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuthorEntityBookEntity_Authors_AuthorsId",
                table: "AuthorEntityBookEntity");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Authors",
                newName: "AuthorId");

            migrationBuilder.RenameColumn(
                name: "AuthorsId",
                table: "AuthorEntityBookEntity",
                newName: "AuthorsAuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuthorEntityBookEntity_Authors_AuthorsAuthorId",
                table: "AuthorEntityBookEntity",
                column: "AuthorsAuthorId",
                principalTable: "Authors",
                principalColumn: "AuthorId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuthorEntityBookEntity_Authors_AuthorsAuthorId",
                table: "AuthorEntityBookEntity");

            migrationBuilder.RenameColumn(
                name: "AuthorId",
                table: "Authors",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "AuthorsAuthorId",
                table: "AuthorEntityBookEntity",
                newName: "AuthorsId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuthorEntityBookEntity_Authors_AuthorsId",
                table: "AuthorEntityBookEntity",
                column: "AuthorsId",
                principalTable: "Authors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
