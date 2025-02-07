using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SyleniumApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class ActiveLedger_RemoveTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_FinancialAccounts_TransactionCategoryId",
                table: "Transactions");

            migrationBuilder.DropTable(
                name: "ActiveLedger");

            migrationBuilder.DropIndex(
                name: "IX_Ledger_Id",
                table: "Ledger");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Ledger",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_FinancialAccountId",
                table: "Transactions",
                column: "FinancialAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Ledger_IsActive",
                table: "Ledger",
                column: "IsActive",
                unique: true,
                filter: "\"IsActive\" = true");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_FinancialAccounts_FinancialAccountId",
                table: "Transactions",
                column: "FinancialAccountId",
                principalTable: "FinancialAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_FinancialAccounts_FinancialAccountId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_FinancialAccountId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Ledger_IsActive",
                table: "Ledger");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Ledger");

            migrationBuilder.CreateTable(
                name: "ActiveLedger",
                columns: table => new
                {
                    LedgerId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActiveLedger", x => x.LedgerId);
                    table.ForeignKey(
                        name: "FK_ActiveLedger_Ledger_LedgerId",
                        column: x => x.LedgerId,
                        principalTable: "Ledger",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ledger_Id",
                table: "Ledger",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_FinancialAccounts_TransactionCategoryId",
                table: "Transactions",
                column: "TransactionCategoryId",
                principalTable: "FinancialAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
