using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class UpdateAdministrators : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdministratorEntity_Users_UserId",
                table: "AdministratorEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AdministratorEntity",
                table: "AdministratorEntity");

            migrationBuilder.RenameTable(
                name: "AdministratorEntity",
                newName: "Administrators");

            migrationBuilder.RenameIndex(
                name: "IX_AdministratorEntity_UserId",
                table: "Administrators",
                newName: "IX_Administrators_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Administrators",
                table: "Administrators",
                column: "AdministartorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Administrators_Users_UserId",
                table: "Administrators",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Administrators_Users_UserId",
                table: "Administrators");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Administrators",
                table: "Administrators");

            migrationBuilder.RenameTable(
                name: "Administrators",
                newName: "AdministratorEntity");

            migrationBuilder.RenameIndex(
                name: "IX_Administrators_UserId",
                table: "AdministratorEntity",
                newName: "IX_AdministratorEntity_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AdministratorEntity",
                table: "AdministratorEntity",
                column: "AdministartorId");

            migrationBuilder.AddForeignKey(
                name: "FK_AdministratorEntity_Users_UserId",
                table: "AdministratorEntity",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
