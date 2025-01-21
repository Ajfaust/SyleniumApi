using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SyleniumApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEntityPropertyNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VendorName",
                table: "Vendor",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "VendorId",
                table: "Vendor",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "TransactionCategoryName",
                table: "TransactionCategory",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "TransactionCategoryId",
                table: "TransactionCategory",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_TransactionCategory_TransactionCategoryName_ParentCategoryId",
                table: "TransactionCategory",
                newName: "IX_TransactionCategory_Name_ParentCategoryId");

            migrationBuilder.RenameColumn(
                name: "TransactionId",
                table: "Transaction",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "LedgerName",
                table: "Ledger",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "LedgerId",
                table: "Ledger",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "FinancialCategoryType",
                table: "FinancialAccountCategory",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "FinancialAccountCategoryName",
                table: "FinancialAccountCategory",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "FinancialAccountCategoryId",
                table: "FinancialAccountCategory",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "FinancialAccountName",
                table: "FinancialAccount",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "FinancialAccountId",
                table: "FinancialAccount",
                newName: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Vendor",
                newName: "VendorName");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Vendor",
                newName: "VendorId");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "TransactionCategory",
                newName: "TransactionCategoryName");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "TransactionCategory",
                newName: "TransactionCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_TransactionCategory_Name_ParentCategoryId",
                table: "TransactionCategory",
                newName: "IX_TransactionCategory_TransactionCategoryName_ParentCategoryId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Transaction",
                newName: "TransactionId");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Ledger",
                newName: "LedgerName");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Ledger",
                newName: "LedgerId");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "FinancialAccountCategory",
                newName: "FinancialCategoryType");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "FinancialAccountCategory",
                newName: "FinancialAccountCategoryName");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "FinancialAccountCategory",
                newName: "FinancialAccountCategoryId");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "FinancialAccount",
                newName: "FinancialAccountName");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "FinancialAccount",
                newName: "FinancialAccountId");
        }
    }
}
