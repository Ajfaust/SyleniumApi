using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SyleniumApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class FinancialAccountCategory_AddFinancialAccountRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_FinancialAccounts_FinancialAccountId1",
                table: "Transaction");

            migrationBuilder.DropIndex(
                name: "IX_Transaction_FinancialAccountId1",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "FinancialAccountId1",
                table: "Transaction");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FinancialAccountId1",
                table: "Transaction",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_FinancialAccountId1",
                table: "Transaction",
                column: "FinancialAccountId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_FinancialAccounts_FinancialAccountId1",
                table: "Transaction",
                column: "FinancialAccountId1",
                principalTable: "FinancialAccounts",
                principalColumn: "Id");
        }
    }
}
