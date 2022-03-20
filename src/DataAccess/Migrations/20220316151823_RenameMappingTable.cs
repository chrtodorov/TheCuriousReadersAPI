using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class RenameMappingTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookAuthors_Books_BookId",
                table: "BookAuthors");

            migrationBuilder.DropForeignKey(
                name: "FK_BookEntityUserEntity_Books_BooksBookId",
                table: "BookEntityUserEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_BookItems_Books_BookId",
                table: "BookItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Books_BlobsMetadata_BlobMetadataId",
                table: "Books");

            migrationBuilder.DropForeignKey(
                name: "FK_Books_Publishers_PublisherId",
                table: "Books");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Books_BookId",
                table: "Comments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Books",
                table: "Books");

            migrationBuilder.RenameTable(
                name: "Books",
                newName: "UsersBooks");

            migrationBuilder.RenameIndex(
                name: "IX_Books_PublisherId",
                table: "UsersBooks",
                newName: "IX_UsersBooks_PublisherId");

            migrationBuilder.RenameIndex(
                name: "IX_Books_Isbn",
                table: "UsersBooks",
                newName: "IX_UsersBooks_Isbn");

            migrationBuilder.RenameIndex(
                name: "IX_Books_BlobMetadataId",
                table: "UsersBooks",
                newName: "IX_UsersBooks_BlobMetadataId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsersBooks",
                table: "UsersBooks",
                column: "BookId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookAuthors_UsersBooks_BookId",
                table: "BookAuthors",
                column: "BookId",
                principalTable: "UsersBooks",
                principalColumn: "BookId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookEntityUserEntity_UsersBooks_BooksBookId",
                table: "BookEntityUserEntity",
                column: "BooksBookId",
                principalTable: "UsersBooks",
                principalColumn: "BookId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookItems_UsersBooks_BookId",
                table: "BookItems",
                column: "BookId",
                principalTable: "UsersBooks",
                principalColumn: "BookId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_UsersBooks_BookId",
                table: "Comments",
                column: "BookId",
                principalTable: "UsersBooks",
                principalColumn: "BookId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersBooks_BlobsMetadata_BlobMetadataId",
                table: "UsersBooks",
                column: "BlobMetadataId",
                principalTable: "BlobsMetadata",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersBooks_Publishers_PublisherId",
                table: "UsersBooks",
                column: "PublisherId",
                principalTable: "Publishers",
                principalColumn: "PublisherId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookAuthors_UsersBooks_BookId",
                table: "BookAuthors");

            migrationBuilder.DropForeignKey(
                name: "FK_BookEntityUserEntity_UsersBooks_BooksBookId",
                table: "BookEntityUserEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_BookItems_UsersBooks_BookId",
                table: "BookItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_UsersBooks_BookId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersBooks_BlobsMetadata_BlobMetadataId",
                table: "UsersBooks");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersBooks_Publishers_PublisherId",
                table: "UsersBooks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UsersBooks",
                table: "UsersBooks");

            migrationBuilder.RenameTable(
                name: "UsersBooks",
                newName: "Books");

            migrationBuilder.RenameIndex(
                name: "IX_UsersBooks_PublisherId",
                table: "Books",
                newName: "IX_Books_PublisherId");

            migrationBuilder.RenameIndex(
                name: "IX_UsersBooks_Isbn",
                table: "Books",
                newName: "IX_Books_Isbn");

            migrationBuilder.RenameIndex(
                name: "IX_UsersBooks_BlobMetadataId",
                table: "Books",
                newName: "IX_Books_BlobMetadataId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Books",
                table: "Books",
                column: "BookId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookAuthors_Books_BookId",
                table: "BookAuthors",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "BookId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookEntityUserEntity_Books_BooksBookId",
                table: "BookEntityUserEntity",
                column: "BooksBookId",
                principalTable: "Books",
                principalColumn: "BookId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookItems_Books_BookId",
                table: "BookItems",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "BookId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Books_BlobsMetadata_BlobMetadataId",
                table: "Books",
                column: "BlobMetadataId",
                principalTable: "BlobsMetadata",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Publishers_PublisherId",
                table: "Books",
                column: "PublisherId",
                principalTable: "Publishers",
                principalColumn: "PublisherId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Books_BookId",
                table: "Comments",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "BookId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
