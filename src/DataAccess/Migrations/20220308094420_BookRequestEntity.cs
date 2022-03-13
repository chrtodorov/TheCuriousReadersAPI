using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class BookRequestEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookLoans_Customers_LoanedBy",
                table: "BookLoans");

            migrationBuilder.DropForeignKey(
                name: "FK_BookLoans_Librarians_ApprovedBy",
                table: "BookLoans");

            migrationBuilder.DropIndex(
                name: "IX_BookLoans_LoanedBy",
                table: "BookLoans");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "BookLoans");

            migrationBuilder.RenameColumn(
                name: "ApprovedBy",
                table: "BookLoans",
                newName: "LibrarianId");

            migrationBuilder.RenameIndex(
                name: "IX_BookLoans_ApprovedBy",
                table: "BookLoans",
                newName: "IX_BookLoans_LibrarianId");

            migrationBuilder.AlterColumn<Guid>(
                name: "LoanedBy",
                table: "BookLoans",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "LoanedTo",
                table: "BookLoans",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "BookRequests",
                columns: table => new
                {
                    BookRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    RequestedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LibrarianId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AuditedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BookItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookRequests", x => x.BookRequestId);
                    table.ForeignKey(
                        name: "FK_BookRequests_BookItems_BookItemId",
                        column: x => x.BookItemId,
                        principalTable: "BookItems",
                        principalColumn: "BookItemId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookRequests_Customers_RequestedBy",
                        column: x => x.RequestedBy,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookRequests_Librarians_LibrarianId",
                        column: x => x.LibrarianId,
                        principalTable: "Librarians",
                        principalColumn: "LibrarianId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookLoans_LoanedTo",
                table: "BookLoans",
                column: "LoanedTo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BookRequests_BookItemId",
                table: "BookRequests",
                column: "BookItemId");

            migrationBuilder.CreateIndex(
                name: "IX_BookRequests_LibrarianId",
                table: "BookRequests",
                column: "LibrarianId");

            migrationBuilder.CreateIndex(
                name: "IX_BookRequests_RequestedBy",
                table: "BookRequests",
                column: "RequestedBy",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BookLoans_Customers_LoanedTo",
                table: "BookLoans",
                column: "LoanedTo",
                principalTable: "Customers",
                principalColumn: "CustomerId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookLoans_Librarians_LibrarianId",
                table: "BookLoans",
                column: "LibrarianId",
                principalTable: "Librarians",
                principalColumn: "LibrarianId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookLoans_Customers_LoanedTo",
                table: "BookLoans");

            migrationBuilder.DropForeignKey(
                name: "FK_BookLoans_Librarians_LibrarianId",
                table: "BookLoans");

            migrationBuilder.DropTable(
                name: "BookRequests");

            migrationBuilder.DropIndex(
                name: "IX_BookLoans_LoanedTo",
                table: "BookLoans");

            migrationBuilder.DropColumn(
                name: "LoanedTo",
                table: "BookLoans");

            migrationBuilder.RenameColumn(
                name: "LibrarianId",
                table: "BookLoans",
                newName: "ApprovedBy");

            migrationBuilder.RenameIndex(
                name: "IX_BookLoans_LibrarianId",
                table: "BookLoans",
                newName: "IX_BookLoans_ApprovedBy");

            migrationBuilder.AlterColumn<Guid>(
                name: "LoanedBy",
                table: "BookLoans",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "BookLoans",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BookLoans_LoanedBy",
                table: "BookLoans",
                column: "LoanedBy",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BookLoans_Customers_LoanedBy",
                table: "BookLoans",
                column: "LoanedBy",
                principalTable: "Customers",
                principalColumn: "CustomerId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookLoans_Librarians_ApprovedBy",
                table: "BookLoans",
                column: "ApprovedBy",
                principalTable: "Librarians",
                principalColumn: "LibrarianId");
        }
    }
}
