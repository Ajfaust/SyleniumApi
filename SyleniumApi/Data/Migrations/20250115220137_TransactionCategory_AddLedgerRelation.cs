using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SyleniumApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class TransactionCategory_AddLedgerRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LedgerId",
                table: "Vendor",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LedgerId",
                table: "TransactionCategory",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LedgerId",
                table: "FinancialAccountCategory",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Vendor_LedgerId",
                table: "Vendor",
                column: "LedgerId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionCategory_LedgerId",
                table: "TransactionCategory",
                column: "LedgerId");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialAccountCategory_LedgerId",
                table: "FinancialAccountCategory",
                column: "LedgerId");

            migrationBuilder.AddForeignKey(
                name: "FK_FinancialAccountCategory_Ledger_LedgerId",
                table: "FinancialAccountCategory",
                column: "LedgerId",
                principalTable: "Ledger",
                principalColumn: "LedgerId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionCategory_Ledger_LedgerId",
                table: "TransactionCategory",
                column: "LedgerId",
                principalTable: "Ledger",
                principalColumn: "LedgerId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Vendor_Ledger_LedgerId",
                table: "Vendor",
                column: "LedgerId",
                principalTable: "Ledger",
                principalColumn: "LedgerId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FinancialAccountCategory_Ledger_LedgerId",
                table: "FinancialAccountCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionCategory_Ledger_LedgerId",
                table: "TransactionCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_Vendor_Ledger_LedgerId",
                table: "Vendor");

            migrationBuilder.DropIndex(
                name: "IX_Vendor_LedgerId",
                table: "Vendor");

            migrationBuilder.DropIndex(
                name: "IX_TransactionCategory_LedgerId",
                table: "TransactionCategory");

            migrationBuilder.DropIndex(
                name: "IX_FinancialAccountCategory_LedgerId",
                table: "FinancialAccountCategory");

            migrationBuilder.DropColumn(
                name: "LedgerId",
                table: "Vendor");

            migrationBuilder.DropColumn(
                name: "LedgerId",
                table: "TransactionCategory");

            migrationBuilder.DropColumn(
                name: "LedgerId",
                table: "FinancialAccountCategory");
        }
    }
}
