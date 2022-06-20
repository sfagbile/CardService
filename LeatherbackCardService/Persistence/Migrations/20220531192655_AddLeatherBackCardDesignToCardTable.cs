using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class AddLeatherBackCardDesignToCardTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LeatherBackCardDesign",
                table: "CardDetails");

            migrationBuilder.AddColumn<string>(
                name: "LeatherBackCardDesign",
                table: "Cards",
                type: "varchar(100)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LeatherBackCardDesign",
                table: "Cards");

            migrationBuilder.AddColumn<string>(
                name: "LeatherBackCardDesign",
                table: "CardDetails",
                type: "varchar(100)",
                nullable: true);
        }
    }
}
