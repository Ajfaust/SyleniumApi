using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SyleniumApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class FinancialAccount_Update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FinancialAccountCategoryId",
                table: "FinancialAccount",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LedgerId",
                table: "FinancialAccount",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_FinancialAccount_FinancialAccountCategoryId",
                table: "FinancialAccount",
                column: "FinancialAccountCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialAccount_LedgerId",
                table: "FinancialAccount",
                column: "LedgerId");

            migrationBuilder.AddForeignKey(
                name: "FK_FinancialAccount_FinancialAccountCategory_FinancialAccountC~",
                table: "FinancialAccount",
                column: "FinancialAccountCategoryId",
                principalTable: "FinancialAccountCategory",
                principalColumn: "FinancialAccountCategoryId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FinancialAccount_Ledger_LedgerId",
                table: "FinancialAccount",
                column: "LedgerId",
                principalTable: "Ledger",
                principalColumn: "LedgerId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FinancialAccount_FinancialAccountCategory_FinancialAccountC~",
                table: "FinancialAccount");

            migrationBuilder.DropForeignKey(
                name: "FK_FinancialAccount_Ledger_LedgerId",
                table: "FinancialAccount");

            migrationBuilder.DropIndex(
                name: "IX_FinancialAccount_FinancialAccountCategoryId",
                table: "FinancialAccount");

            migrationBuilder.DropIndex(
                name: "IX_FinancialAccount_LedgerId",
                table: "FinancialAccount");

            migrationBuilder.DropColumn(
                name: "FinancialAccountCategoryId",
                table: "FinancialAccount");

            migrationBuilder.DropColumn(
                name: "LedgerId",
                table: "FinancialAccount");
        }
    }
}
