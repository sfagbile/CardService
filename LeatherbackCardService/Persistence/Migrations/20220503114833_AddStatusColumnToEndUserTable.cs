using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class AddStatusColumnToEndUserTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CardRequestId",
                table: "ProviderEndUsers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "ProviderEndUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_ProviderEndUsers_CardRequestId",
                table: "ProviderEndUsers",
                column: "CardRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProviderEndUsers_CardRequests_CardRequestId",
                table: "ProviderEndUsers",
                column: "CardRequestId",
                principalTable: "CardRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProviderEndUsers_CardRequests_CardRequestId",
                table: "ProviderEndUsers");

            migrationBuilder.DropIndex(
                name: "IX_ProviderEndUsers_CardRequestId",
                table: "ProviderEndUsers");

            migrationBuilder.DropColumn(
                name: "CardRequestId",
                table: "ProviderEndUsers");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "ProviderEndUsers");
        }
    }
}
