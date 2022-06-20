using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class IncreaseCardIdentifierLengthOnCardTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CardIdentifier",
                table: "Cards",
                type: "Varchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "Varchar(30)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CardIdentifier",
                table: "Cards",
                type: "Varchar(30)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "Varchar(50)");
        }
    }
}
