using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SyleniumApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class Fix_FinancialAccountCategory_Table_Name : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_FinancialCategory",
                table: "FinancialCategory");

            migrationBuilder.RenameTable(
                name: "FinancialCategory",
                newName: "FinancialAccountCategory");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FinancialAccountCategory",
                table: "FinancialAccountCategory",
                column: "FinancialAccountCategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_FinancialAccountCategory",
                table: "FinancialAccountCategory");

            migrationBuilder.RenameTable(
                name: "FinancialAccountCategory",
                newName: "FinancialCategory");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FinancialCategory",
                table: "FinancialCategory",
                column: "FinancialAccountCategoryId");
        }
    }
}
