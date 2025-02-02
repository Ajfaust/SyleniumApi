using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SyleniumApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class MoveToFluentApi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_FinancialAccounts_FinancialAccountId",
                table: "Transaction");

            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_TransactionCategory_TransactionCategoryId",
                table: "Transaction");

            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Vendor_VendorId",
                table: "Transaction");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionCategory_Ledger_LedgerId",
                table: "TransactionCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionCategory_TransactionCategory_ParentCategoryId",
                table: "TransactionCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_Vendor_Ledger_LedgerId",
                table: "Vendor");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Vendor",
                table: "Vendor");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TransactionCategory",
                table: "TransactionCategory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Transaction",
                table: "Transaction");

            migrationBuilder.DropIndex(
                name: "IX_Transaction_FinancialAccountId",
                table: "Transaction");

            migrationBuilder.RenameTable(
                name: "Vendor",
                newName: "Vendors");

            migrationBuilder.RenameTable(
                name: "TransactionCategory",
                newName: "TransactionCategories");

            migrationBuilder.RenameTable(
                name: "Transaction",
                newName: "Transactions");

            migrationBuilder.RenameIndex(
                name: "IX_Vendor_LedgerId",
                table: "Vendors",
                newName: "IX_Vendors_LedgerId");

            migrationBuilder.RenameIndex(
                name: "IX_TransactionCategory_ParentCategoryId",
                table: "TransactionCategories",
                newName: "IX_TransactionCategories_ParentCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_TransactionCategory_Name_ParentCategoryId",
                table: "TransactionCategories",
                newName: "IX_TransactionCategories_Name_ParentCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_TransactionCategory_LedgerId",
                table: "TransactionCategories",
                newName: "IX_TransactionCategories_LedgerId");

            migrationBuilder.RenameIndex(
                name: "IX_Transaction_VendorId",
                table: "Transactions",
                newName: "IX_Transactions_VendorId");

            migrationBuilder.RenameIndex(
                name: "IX_Transaction_TransactionCategoryId",
                table: "Transactions",
                newName: "IX_Transactions_TransactionCategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Vendors",
                table: "Vendors",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TransactionCategories",
                table: "TransactionCategories",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Transactions",
                table: "Transactions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionCategories_Ledger_LedgerId",
                table: "TransactionCategories",
                column: "LedgerId",
                principalTable: "Ledger",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionCategories_TransactionCategories_ParentCategoryId",
                table: "TransactionCategories",
                column: "ParentCategoryId",
                principalTable: "TransactionCategories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_FinancialAccounts_TransactionCategoryId",
                table: "Transactions",
                column: "TransactionCategoryId",
                principalTable: "FinancialAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_TransactionCategories_TransactionCategoryId",
                table: "Transactions",
                column: "TransactionCategoryId",
                principalTable: "TransactionCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Vendors_VendorId",
                table: "Transactions",
                column: "VendorId",
                principalTable: "Vendors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Vendors_Ledger_LedgerId",
                table: "Vendors",
                column: "LedgerId",
                principalTable: "Ledger",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransactionCategories_Ledger_LedgerId",
                table: "TransactionCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionCategories_TransactionCategories_ParentCategoryId",
                table: "TransactionCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_FinancialAccounts_TransactionCategoryId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_TransactionCategories_TransactionCategoryId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Vendors_VendorId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Vendors_Ledger_LedgerId",
                table: "Vendors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Vendors",
                table: "Vendors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Transactions",
                table: "Transactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TransactionCategories",
                table: "TransactionCategories");

            migrationBuilder.RenameTable(
                name: "Vendors",
                newName: "Vendor");

            migrationBuilder.RenameTable(
                name: "Transactions",
                newName: "Transaction");

            migrationBuilder.RenameTable(
                name: "TransactionCategories",
                newName: "TransactionCategory");

            migrationBuilder.RenameIndex(
                name: "IX_Vendors_LedgerId",
                table: "Vendor",
                newName: "IX_Vendor_LedgerId");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_VendorId",
                table: "Transaction",
                newName: "IX_Transaction_VendorId");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_TransactionCategoryId",
                table: "Transaction",
                newName: "IX_Transaction_TransactionCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_TransactionCategories_ParentCategoryId",
                table: "TransactionCategory",
                newName: "IX_TransactionCategory_ParentCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_TransactionCategories_Name_ParentCategoryId",
                table: "TransactionCategory",
                newName: "IX_TransactionCategory_Name_ParentCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_TransactionCategories_LedgerId",
                table: "TransactionCategory",
                newName: "IX_TransactionCategory_LedgerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Vendor",
                table: "Vendor",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Transaction",
                table: "Transaction",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TransactionCategory",
                table: "TransactionCategory",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_FinancialAccountId",
                table: "Transaction",
                column: "FinancialAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_FinancialAccounts_FinancialAccountId",
                table: "Transaction",
                column: "FinancialAccountId",
                principalTable: "FinancialAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_TransactionCategory_TransactionCategoryId",
                table: "Transaction",
                column: "TransactionCategoryId",
                principalTable: "TransactionCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Vendor_VendorId",
                table: "Transaction",
                column: "VendorId",
                principalTable: "Vendor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionCategory_Ledger_LedgerId",
                table: "TransactionCategory",
                column: "LedgerId",
                principalTable: "Ledger",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionCategory_TransactionCategory_ParentCategoryId",
                table: "TransactionCategory",
                column: "ParentCategoryId",
                principalTable: "TransactionCategory",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Vendor_Ledger_LedgerId",
                table: "Vendor",
                column: "LedgerId",
                principalTable: "Ledger",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
