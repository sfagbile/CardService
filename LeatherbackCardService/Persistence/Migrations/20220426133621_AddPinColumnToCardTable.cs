using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class AddPinColumnToCardTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CustomerIdentity",
                table: "CardRequests",
                newName: "CustomerId");

            migrationBuilder.AddColumn<string>(
                name: "Pin",
                table: "Cards",
                type: "Varchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AccountNumber",
                table: "CardRequests",
                type: "varchar(30)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Pin",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "AccountNumber",
                table: "CardRequests");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "CardRequests",
                newName: "CustomerIdentity");
        }
    }
}
