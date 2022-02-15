using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class AddUserStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Approved",
                table: "Librarians");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Librarians",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Customers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Librarians");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Customers");

            migrationBuilder.AddColumn<bool>(
                name: "Approved",
                table: "Librarians",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
