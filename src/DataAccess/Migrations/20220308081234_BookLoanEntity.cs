using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class BookLoanEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BookLoanId",
                table: "BookItems",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BookLoans",
                columns: table => new
                {
                    BookLoanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    From = table.Column<DateTime>(type: "datetime2", nullable: false),
                    To = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TimesExtended = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: true),
                    LoanedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApprovedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookLoans", x => x.BookLoanId);
                    table.ForeignKey(
                        name: "FK_BookLoans_Customers_LoanedBy",
                        column: x => x.LoanedBy,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookLoans_Librarians_ApprovedBy",
                        column: x => x.ApprovedBy,
                        principalTable: "Librarians",
                        principalColumn: "LibrarianId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookItems_BookLoanId",
                table: "BookItems",
                column: "BookLoanId",
                unique: true,
                filter: "[BookLoanId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BookLoans_ApprovedBy",
                table: "BookLoans",
                column: "ApprovedBy");

            migrationBuilder.CreateIndex(
                name: "IX_BookLoans_LoanedBy",
                table: "BookLoans",
                column: "LoanedBy",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BookItems_BookLoans_BookLoanId",
                table: "BookItems",
                column: "BookLoanId",
                principalTable: "BookLoans",
                principalColumn: "BookLoanId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookItems_BookLoans_BookLoanId",
                table: "BookItems");

            migrationBuilder.DropTable(
                name: "BookLoans");

            migrationBuilder.DropIndex(
                name: "IX_BookItems_BookLoanId",
                table: "BookItems");

            migrationBuilder.DropColumn(
                name: "BookLoanId",
                table: "BookItems");
        }
    }
}
