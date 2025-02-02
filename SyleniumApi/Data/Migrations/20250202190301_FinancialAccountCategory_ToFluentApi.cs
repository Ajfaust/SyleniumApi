using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SyleniumApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class FinancialAccountCategory_ToFluentApi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FinancialAccountCategory_Ledger_LedgerId",
                table: "FinancialAccountCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_FinancialAccounts_FinancialAccountCategory_FinancialAccount~",
                table: "FinancialAccounts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FinancialAccountCategory",
                table: "FinancialAccountCategory");

            migrationBuilder.RenameTable(
                name: "FinancialAccountCategory",
                newName: "FinancialAccountCategories");

            migrationBuilder.RenameIndex(
                name: "IX_FinancialAccountCategory_LedgerId",
                table: "FinancialAccountCategories",
                newName: "IX_FinancialAccountCategories_LedgerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FinancialAccountCategories",
                table: "FinancialAccountCategories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FinancialAccountCategories_Ledger_LedgerId",
                table: "FinancialAccountCategories",
                column: "LedgerId",
                principalTable: "Ledger",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FinancialAccounts_FinancialAccountCategories_FinancialAccou~",
                table: "FinancialAccounts",
                column: "FinancialAccountCategoryId",
                principalTable: "FinancialAccountCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FinancialAccountCategories_Ledger_LedgerId",
                table: "FinancialAccountCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_FinancialAccounts_FinancialAccountCategories_FinancialAccou~",
                table: "FinancialAccounts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FinancialAccountCategories",
                table: "FinancialAccountCategories");

            migrationBuilder.RenameTable(
                name: "FinancialAccountCategories",
                newName: "FinancialAccountCategory");

            migrationBuilder.RenameIndex(
                name: "IX_FinancialAccountCategories_LedgerId",
                table: "FinancialAccountCategory",
                newName: "IX_FinancialAccountCategory_LedgerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FinancialAccountCategory",
                table: "FinancialAccountCategory",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FinancialAccountCategory_Ledger_LedgerId",
                table: "FinancialAccountCategory",
                column: "LedgerId",
                principalTable: "Ledger",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FinancialAccounts_FinancialAccountCategory_FinancialAccount~",
                table: "FinancialAccounts",
                column: "FinancialAccountCategoryId",
                principalTable: "FinancialAccountCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
