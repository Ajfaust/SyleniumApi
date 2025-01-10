using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SyleniumApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class Account_FinancialCategory_RenameTo_FinancialAccount_FinancialAccountCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Account_AccountId",
                table: "Transaction");

            migrationBuilder.DropTable(
                name: "Account");

            migrationBuilder.DropIndex(
                name: "IX_Transaction_AccountId",
                table: "Transaction");

            migrationBuilder.RenameColumn(
                name: "FinancialCategoryName",
                table: "FinancialCategory",
                newName: "FinancialAccountCategoryName");

            migrationBuilder.RenameColumn(
                name: "FinancialCategoryId",
                table: "FinancialCategory",
                newName: "FinancialAccountCategoryId");

            migrationBuilder.AddColumn<int>(
                name: "FinancialAccountId",
                table: "Transaction",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FinancialCategoryType",
                table: "FinancialCategory",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "FinancialAccount",
                columns: table => new
                {
                    FinancialAccountId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FinancialAccountName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialAccount", x => x.FinancialAccountId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_FinancialAccountId",
                table: "Transaction",
                column: "FinancialAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_FinancialAccount_FinancialAccountId",
                table: "Transaction",
                column: "FinancialAccountId",
                principalTable: "FinancialAccount",
                principalColumn: "FinancialAccountId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_FinancialAccount_FinancialAccountId",
                table: "Transaction");

            migrationBuilder.DropTable(
                name: "FinancialAccount");

            migrationBuilder.DropIndex(
                name: "IX_Transaction_FinancialAccountId",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "FinancialAccountId",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "FinancialCategoryType",
                table: "FinancialCategory");

            migrationBuilder.RenameColumn(
                name: "FinancialAccountCategoryName",
                table: "FinancialCategory",
                newName: "FinancialCategoryName");

            migrationBuilder.RenameColumn(
                name: "FinancialAccountCategoryId",
                table: "FinancialCategory",
                newName: "FinancialCategoryId");

            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    AccountId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AccountName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.AccountId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_AccountId",
                table: "Transaction",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Account_AccountId",
                table: "Transaction",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "AccountId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
