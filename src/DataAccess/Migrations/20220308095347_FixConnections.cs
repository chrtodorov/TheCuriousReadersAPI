using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class FixConnections : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookLoans_Librarians_LibrarianId",
                table: "BookLoans");

            migrationBuilder.DropIndex(
                name: "IX_BookLoans_LibrarianId",
                table: "BookLoans");

            migrationBuilder.DropColumn(
                name: "LibrarianId",
                table: "BookLoans");

            migrationBuilder.AlterColumn<Guid>(
                name: "LoanedBy",
                table: "BookLoans",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateIndex(
                name: "IX_BookLoans_LoanedBy",
                table: "BookLoans",
                column: "LoanedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_BookLoans_Librarians_LoanedBy",
                table: "BookLoans",
                column: "LoanedBy",
                principalTable: "Librarians",
                principalColumn: "LibrarianId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookLoans_Librarians_LoanedBy",
                table: "BookLoans");

            migrationBuilder.DropIndex(
                name: "IX_BookLoans_LoanedBy",
                table: "BookLoans");

            migrationBuilder.AlterColumn<Guid>(
                name: "LoanedBy",
                table: "BookLoans",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LibrarianId",
                table: "BookLoans",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_BookLoans_LibrarianId",
                table: "BookLoans",
                column: "LibrarianId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookLoans_Librarians_LibrarianId",
                table: "BookLoans",
                column: "LibrarianId",
                principalTable: "Librarians",
                principalColumn: "LibrarianId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
