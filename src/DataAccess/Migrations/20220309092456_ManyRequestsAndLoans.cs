using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class ManyRequestsAndLoans : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BookRequests_RequestedBy",
                table: "BookRequests");

            migrationBuilder.DropIndex(
                name: "IX_BookLoans_LoanedTo",
                table: "BookLoans");

            migrationBuilder.CreateIndex(
                name: "IX_BookRequests_RequestedBy",
                table: "BookRequests",
                column: "RequestedBy");

            migrationBuilder.CreateIndex(
                name: "IX_BookLoans_LoanedTo",
                table: "BookLoans",
                column: "LoanedTo");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BookRequests_RequestedBy",
                table: "BookRequests");

            migrationBuilder.DropIndex(
                name: "IX_BookLoans_LoanedTo",
                table: "BookLoans");

            migrationBuilder.CreateIndex(
                name: "IX_BookRequests_RequestedBy",
                table: "BookRequests",
                column: "RequestedBy",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BookLoans_LoanedTo",
                table: "BookLoans",
                column: "LoanedTo",
                unique: true);
        }
    }
}
