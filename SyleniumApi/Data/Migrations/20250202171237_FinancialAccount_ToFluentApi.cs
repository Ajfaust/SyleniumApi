using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SyleniumApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class FinancialAccount_ToFluentApi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FinancialAccount_FinancialAccountCategory_FinancialAccountC~",
                table: "FinancialAccount");

            migrationBuilder.DropForeignKey(
                name: "FK_FinancialAccount_Ledger_LedgerId",
                table: "FinancialAccount");

            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_FinancialAccount_FinancialAccountId",
                table: "Transaction");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FinancialAccount",
                table: "FinancialAccount");

            migrationBuilder.RenameTable(
                name: "FinancialAccount",
                newName: "FinancialAccounts");

            migrationBuilder.RenameIndex(
                name: "IX_FinancialAccount_LedgerId",
                table: "FinancialAccounts",
                newName: "IX_FinancialAccounts_LedgerId");

            migrationBuilder.RenameIndex(
                name: "IX_FinancialAccount_FinancialAccountCategoryId",
                table: "FinancialAccounts",
                newName: "IX_FinancialAccounts_FinancialAccountCategoryId");

            migrationBuilder.AddColumn<int>(
                name: "FinancialAccountId1",
                table: "Transaction",
                type: "integer",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_FinancialAccounts",
                table: "FinancialAccounts",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_FinancialAccountId1",
                table: "Transaction",
                column: "FinancialAccountId1");

            migrationBuilder.AddForeignKey(
                name: "FK_FinancialAccounts_FinancialAccountCategory_FinancialAccount~",
                table: "FinancialAccounts",
                column: "FinancialAccountCategoryId",
                principalTable: "FinancialAccountCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FinancialAccounts_Ledger_LedgerId",
                table: "FinancialAccounts",
                column: "LedgerId",
                principalTable: "Ledger",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_FinancialAccounts_FinancialAccountId",
                table: "Transaction",
                column: "FinancialAccountId",
                principalTable: "FinancialAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_FinancialAccounts_FinancialAccountId1",
                table: "Transaction",
                column: "FinancialAccountId1",
                principalTable: "FinancialAccounts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FinancialAccounts_FinancialAccountCategory_FinancialAccount~",
                table: "FinancialAccounts");

            migrationBuilder.DropForeignKey(
                name: "FK_FinancialAccounts_Ledger_LedgerId",
                table: "FinancialAccounts");

            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_FinancialAccounts_FinancialAccountId",
                table: "Transaction");

            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_FinancialAccounts_FinancialAccountId1",
                table: "Transaction");

            migrationBuilder.DropIndex(
                name: "IX_Transaction_FinancialAccountId1",
                table: "Transaction");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FinancialAccounts",
                table: "FinancialAccounts");

            migrationBuilder.DropColumn(
                name: "FinancialAccountId1",
                table: "Transaction");

            migrationBuilder.RenameTable(
                name: "FinancialAccounts",
                newName: "FinancialAccount");

            migrationBuilder.RenameIndex(
                name: "IX_FinancialAccounts_LedgerId",
                table: "FinancialAccount",
                newName: "IX_FinancialAccount_LedgerId");

            migrationBuilder.RenameIndex(
                name: "IX_FinancialAccounts_FinancialAccountCategoryId",
                table: "FinancialAccount",
                newName: "IX_FinancialAccount_FinancialAccountCategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FinancialAccount",
                table: "FinancialAccount",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FinancialAccount_FinancialAccountCategory_FinancialAccountC~",
                table: "FinancialAccount",
                column: "FinancialAccountCategoryId",
                principalTable: "FinancialAccountCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FinancialAccount_Ledger_LedgerId",
                table: "FinancialAccount",
                column: "LedgerId",
                principalTable: "Ledger",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_FinancialAccount_FinancialAccountId",
                table: "Transaction",
                column: "FinancialAccountId",
                principalTable: "FinancialAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
