using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class BookItems_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_BookItems",
                table: "BookItems");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "BookItems");

            migrationBuilder.AddColumn<Guid>(
                name: "BookItemId",
                table: "BookItems",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookItems",
                table: "BookItems",
                column: "BookItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_BookItems",
                table: "BookItems");

            migrationBuilder.DropColumn(
                name: "BookItemId",
                table: "BookItems");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "BookItems",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookItems",
                table: "BookItems",
                column: "Id");
        }
    }
}
