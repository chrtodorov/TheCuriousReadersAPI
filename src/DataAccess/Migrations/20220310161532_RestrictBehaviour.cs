using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class RestrictBehaviour : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookLoans_Librarians_LoanedBy",
                table: "BookLoans");

            migrationBuilder.AddForeignKey(
                name: "FK_BookLoans_Librarians_LoanedBy",
                table: "BookLoans",
                column: "LoanedBy",
                principalTable: "Librarians",
                principalColumn: "LibrarianId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookLoans_Librarians_LoanedBy",
                table: "BookLoans");

            migrationBuilder.AddForeignKey(
                name: "FK_BookLoans_Librarians_LoanedBy",
                table: "BookLoans",
                column: "LoanedBy",
                principalTable: "Librarians",
                principalColumn: "LibrarianId");
        }
    }
}
