using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class AddBookToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuthorEntityBookEntity_Books_BooksId",
                table: "AuthorEntityBookEntity");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Books",
                newName: "BookId");

            migrationBuilder.RenameColumn(
                name: "BooksId",
                table: "AuthorEntityBookEntity",
                newName: "BooksBookId");

            migrationBuilder.RenameIndex(
                name: "IX_AuthorEntityBookEntity_BooksId",
                table: "AuthorEntityBookEntity",
                newName: "IX_AuthorEntityBookEntity_BooksBookId");

            migrationBuilder.AlterColumn<string>(
                name: "Isbn",
                table: "Books",
                type: "nvarchar(17)",
                maxLength: 17,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(13)",
                oldMaxLength: 13);

            migrationBuilder.AddForeignKey(
                name: "FK_AuthorEntityBookEntity_Books_BooksBookId",
                table: "AuthorEntityBookEntity",
                column: "BooksBookId",
                principalTable: "Books",
                principalColumn: "BookId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuthorEntityBookEntity_Books_BooksBookId",
                table: "AuthorEntityBookEntity");

            migrationBuilder.RenameColumn(
                name: "BookId",
                table: "Books",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "BooksBookId",
                table: "AuthorEntityBookEntity",
                newName: "BooksId");

            migrationBuilder.RenameIndex(
                name: "IX_AuthorEntityBookEntity_BooksBookId",
                table: "AuthorEntityBookEntity",
                newName: "IX_AuthorEntityBookEntity_BooksId");

            migrationBuilder.AlterColumn<string>(
                name: "Isbn",
                table: "Books",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(17)",
                oldMaxLength: 17);

            migrationBuilder.AddForeignKey(
                name: "FK_AuthorEntityBookEntity_Books_BooksId",
                table: "AuthorEntityBookEntity",
                column: "BooksId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
