using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class AddBlobMetadata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CoverUrl",
                table: "Books",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<Guid>(
                name: "BlobMetadataId",
                table: "Books",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BlobsMetadata",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BlobName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlobsMetadata", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Books_BlobMetadataId",
                table: "Books",
                column: "BlobMetadataId",
                unique: true,
                filter: "[BlobMetadataId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_BlobsMetadata_BlobMetadataId",
                table: "Books",
                column: "BlobMetadataId",
                principalTable: "BlobsMetadata",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_BlobsMetadata_BlobMetadataId",
                table: "Books");

            migrationBuilder.DropTable(
                name: "BlobsMetadata");

            migrationBuilder.DropIndex(
                name: "IX_Books_BlobMetadataId",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "BlobMetadataId",
                table: "Books");

            migrationBuilder.AlterColumn<string>(
                name: "CoverUrl",
                table: "Books",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);
        }
    }
}
