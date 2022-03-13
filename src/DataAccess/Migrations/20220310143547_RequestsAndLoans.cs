using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class RequestsAndLoans : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookItems_BookLoans_BookLoanId",
                table: "BookItems");

            migrationBuilder.DropForeignKey(
                name: "FK_BookRequests_Books_BookId",
                table: "BookRequests");

            migrationBuilder.RenameColumn(
                name: "BookId",
                table: "BookRequests",
                newName: "BookItemId");

            migrationBuilder.RenameIndex(
                name: "IX_BookRequests_BookId",
                table: "BookRequests",
                newName: "IX_BookRequests_BookItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookItems_BookLoans_BookLoanId",
                table: "BookItems",
                column: "BookLoanId",
                principalTable: "BookLoans",
                principalColumn: "BookLoanId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BookRequests_BookItems_BookItemId",
                table: "BookRequests",
                column: "BookItemId",
                principalTable: "BookItems",
                principalColumn: "BookItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookItems_BookLoans_BookLoanId",
                table: "BookItems");

            migrationBuilder.DropForeignKey(
                name: "FK_BookRequests_BookItems_BookItemId",
                table: "BookRequests");

            migrationBuilder.RenameColumn(
                name: "BookItemId",
                table: "BookRequests",
                newName: "BookId");

            migrationBuilder.RenameIndex(
                name: "IX_BookRequests_BookItemId",
                table: "BookRequests",
                newName: "IX_BookRequests_BookId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookItems_BookLoans_BookLoanId",
                table: "BookItems",
                column: "BookLoanId",
                principalTable: "BookLoans",
                principalColumn: "BookLoanId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookRequests_Books_BookId",
                table: "BookRequests",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "BookId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
