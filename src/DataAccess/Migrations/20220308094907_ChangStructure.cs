using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class ChangStructure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookRequests_Librarians_LibrarianId",
                table: "BookRequests");

            migrationBuilder.DropIndex(
                name: "IX_BookRequests_LibrarianId",
                table: "BookRequests");

            migrationBuilder.DropColumn(
                name: "LibrarianId",
                table: "BookRequests");

            migrationBuilder.CreateIndex(
                name: "IX_BookRequests_AuditedBy",
                table: "BookRequests",
                column: "AuditedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_BookRequests_Librarians_AuditedBy",
                table: "BookRequests",
                column: "AuditedBy",
                principalTable: "Librarians",
                principalColumn: "LibrarianId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookRequests_Librarians_AuditedBy",
                table: "BookRequests");

            migrationBuilder.DropIndex(
                name: "IX_BookRequests_AuditedBy",
                table: "BookRequests");

            migrationBuilder.AddColumn<Guid>(
                name: "LibrarianId",
                table: "BookRequests",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BookRequests_LibrarianId",
                table: "BookRequests",
                column: "LibrarianId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookRequests_Librarians_LibrarianId",
                table: "BookRequests",
                column: "LibrarianId",
                principalTable: "Librarians",
                principalColumn: "LibrarianId");
        }
    }
}
