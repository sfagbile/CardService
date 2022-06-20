using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class AddTruncatedPancolumnToCardTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Pin",
                table: "Cards");

            migrationBuilder.AlterColumn<string>(
                name: "CardNumber",
                table: "Cards",
                type: "varchar(30)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(20)",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CardLimit",
                table: "Cards",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "ExpiryDate",
                table: "Cards",
                type: "Varchar(30)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MaskedPan",
                table: "Cards",
                type: "Varchar(30)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TruncatedPan",
                table: "Cards",
                type: "Varchar(30)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CardLimit",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "ExpiryDate",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "MaskedPan",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "TruncatedPan",
                table: "Cards");

            migrationBuilder.AlterColumn<string>(
                name: "CardNumber",
                table: "Cards",
                type: "varchar(20)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(30)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Pin",
                table: "Cards",
                type: "Varchar(max)",
                nullable: true);
        }
    }
}
