using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class AddCreateProviderEndUserResponseColumnToCardRequestTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreateLedgerResponse",
                table: "CardRequests",
                newName: "CreateProviderEndUserResponse");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreateProviderEndUserResponse",
                table: "CardRequests",
                newName: "CreateLedgerResponse");
        }
    }
}
