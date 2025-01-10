using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SyleniumApi.Migrations
{
    /// <inheritdoc />
    public partial class RefactorForUpdatedSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Account_AccountId",
                table: "Transaction");

            migrationBuilder.DropIndex(
                name: "IX_TransactionCategory_Name_ParentCategoryId",
                table: "TransactionCategory");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Vendor");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "TransactionCategory");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "Balance",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Account");

            migrationBuilder.AddColumn<string>(
                name: "VendorName",
                table: "Vendor",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TransactionCategoryName",
                table: "TransactionCategory",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "AccountId",
                table: "Transaction",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Transaction",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AccountName",
                table: "Account",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionCategory_TransactionCategoryName_ParentCategoryId",
                table: "TransactionCategory",
                columns: new[] { "TransactionCategoryName", "ParentCategoryId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Account_AccountId",
                table: "Transaction",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "AccountId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Account_AccountId",
                table: "Transaction");

            migrationBuilder.DropIndex(
                name: "IX_TransactionCategory_TransactionCategoryName_ParentCategoryId",
                table: "TransactionCategory");

            migrationBuilder.DropColumn(
                name: "VendorName",
                table: "Vendor");

            migrationBuilder.DropColumn(
                name: "TransactionCategoryName",
                table: "TransactionCategory");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "AccountName",
                table: "Account");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Vendor",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "TransactionCategory",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "AccountId",
                table: "Transaction",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Transaction",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Balance",
                table: "Account",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Account",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionCategory_Name_ParentCategoryId",
                table: "TransactionCategory",
                columns: new[] { "Name", "ParentCategoryId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Account_AccountId",
                table: "Transaction",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "AccountId");
        }
    }
}
