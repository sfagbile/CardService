using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class CreateCardDetailTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cards_CustomerCardDetails_CustomerCardDetailId",
                table: "Cards");

            migrationBuilder.DropTable(
                name: "CustomerCardDetails");

            migrationBuilder.RenameColumn(
                name: "CustomerCardDetailId",
                table: "Cards",
                newName: "CardDetailId");

            migrationBuilder.RenameIndex(
                name: "IX_Cards_CustomerCardDetailId",
                table: "Cards",
                newName: "IX_Cards_CardDetailId");

            migrationBuilder.RenameColumn(
                name: "IsCreateLedgerSuccessful",
                table: "CardRequests",
                newName: "IsCreateProviderEndUserSuccessful");

            migrationBuilder.RenameColumn(
                name: "IsCreateLedgerInitiated",
                table: "CardRequests",
                newName: "IsCreateProviderEndUserInitiated");

            migrationBuilder.RenameColumn(
                name: "IsCreateCustomerCardDetailsSuccessful",
                table: "CardRequests",
                newName: "IsCreateCardDetailsSuccessful");

            migrationBuilder.RenameColumn(
                name: "IsCreateCustomerCardDetailsInitiated",
                table: "CardRequests",
                newName: "IsCreateCardDetailsInitiated");

            migrationBuilder.RenameColumn(
                name: "CreateCustomerCardDetailsResponse",
                table: "CardRequests",
                newName: "CreateCardDetailsResponse");

            migrationBuilder.CreateTable(
                name: "ProviderEndUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EndUserId = table.Column<string>(type: "varchar(50)", nullable: true),
                    CardProviderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProviderEndUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProviderEndUsers_CardProviders_CardProviderId",
                        column: x => x.CardProviderId,
                        principalTable: "CardProviders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProviderEndUsers_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CardDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProviderEndUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProviderLedgerId = table.Column<string>(type: "varchar(50)", nullable: true),
                    CardType = table.Column<string>(type: "Varchar(30)", nullable: false),
                    CurrencyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CardRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CardDesign = table.Column<string>(type: "varchar(50)", nullable: false),
                    CardProgramme = table.Column<string>(type: "varchar(50)", nullable: false),
                    Status = table.Column<string>(type: "Varchar(30)", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CardDetails_CardRequests_CardRequestId",
                        column: x => x.CardRequestId,
                        principalTable: "CardRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CardDetails_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CardDetails_ProviderEndUsers_ProviderEndUserId",
                        column: x => x.ProviderEndUserId,
                        principalTable: "ProviderEndUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CardDetails_CardRequestId",
                table: "CardDetails",
                column: "CardRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_CardDetails_CurrencyId",
                table: "CardDetails",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_CardDetails_ProviderEndUserId",
                table: "CardDetails",
                column: "ProviderEndUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProviderEndUsers_CardProviderId",
                table: "ProviderEndUsers",
                column: "CardProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_ProviderEndUsers_CustomerId",
                table: "ProviderEndUsers",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_CardDetails_CardDetailId",
                table: "Cards",
                column: "CardDetailId",
                principalTable: "CardDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cards_CardDetails_CardDetailId",
                table: "Cards");

            migrationBuilder.DropTable(
                name: "CardDetails");

            migrationBuilder.DropTable(
                name: "ProviderEndUsers");

            migrationBuilder.RenameColumn(
                name: "CardDetailId",
                table: "Cards",
                newName: "CustomerCardDetailId");

            migrationBuilder.RenameIndex(
                name: "IX_Cards_CardDetailId",
                table: "Cards",
                newName: "IX_Cards_CustomerCardDetailId");

            migrationBuilder.RenameColumn(
                name: "IsCreateProviderEndUserSuccessful",
                table: "CardRequests",
                newName: "IsCreateLedgerSuccessful");

            migrationBuilder.RenameColumn(
                name: "IsCreateProviderEndUserInitiated",
                table: "CardRequests",
                newName: "IsCreateLedgerInitiated");

            migrationBuilder.RenameColumn(
                name: "IsCreateCardDetailsSuccessful",
                table: "CardRequests",
                newName: "IsCreateCustomerCardDetailsSuccessful");

            migrationBuilder.RenameColumn(
                name: "IsCreateCardDetailsInitiated",
                table: "CardRequests",
                newName: "IsCreateCustomerCardDetailsInitiated");

            migrationBuilder.RenameColumn(
                name: "CreateCardDetailsResponse",
                table: "CardRequests",
                newName: "CreateCustomerCardDetailsResponse");

            migrationBuilder.CreateTable(
                name: "CustomerCardDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CardDesign = table.Column<string>(type: "varchar(50)", nullable: false),
                    CardProgramme = table.Column<string>(type: "varchar(50)", nullable: false),
                    CardProviderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CardRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CardType = table.Column<string>(type: "Varchar(30)", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurrencyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerType = table.Column<string>(type: "Varchar(30)", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProviderEndUserId = table.Column<string>(type: "varchar(50)", nullable: true),
                    ProviderLedgerId = table.Column<string>(type: "varchar(50)", nullable: true),
                    Status = table.Column<string>(type: "Varchar(30)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerCardDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerCardDetails_CardProviders_CardProviderId",
                        column: x => x.CardProviderId,
                        principalTable: "CardProviders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerCardDetails_CardRequests_CardRequestId",
                        column: x => x.CardRequestId,
                        principalTable: "CardRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerCardDetails_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerCardDetails_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerCardDetails_CardProviderId",
                table: "CustomerCardDetails",
                column: "CardProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerCardDetails_CardRequestId",
                table: "CustomerCardDetails",
                column: "CardRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerCardDetails_CurrencyId",
                table: "CustomerCardDetails",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerCardDetails_CustomerId",
                table: "CustomerCardDetails",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_CustomerCardDetails_CustomerCardDetailId",
                table: "Cards",
                column: "CustomerCardDetailId",
                principalTable: "CustomerCardDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
