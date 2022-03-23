using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class CascadeDeleteBookRequestsAndBookLoanStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookRequests_BookItems_BookItemId",
                table: "BookRequests");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "BookLoans",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_BookRequests_BookItems_BookItemId",
                table: "BookRequests",
                column: "BookItemId",
                principalTable: "BookItems",
                principalColumn: "BookItemId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookRequests_BookItems_BookItemId",
                table: "BookRequests");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "BookLoans");

            migrationBuilder.AddForeignKey(
                name: "FK_BookRequests_BookItems_BookItemId",
                table: "BookRequests",
                column: "BookItemId",
                principalTable: "BookItems",
                principalColumn: "BookItemId");
        }
    }
}
