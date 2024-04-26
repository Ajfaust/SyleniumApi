using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BudgetUpServer.Migrations
{
    /// <inheritdoc />
    public partial class SeedAccountTypeAndCategoryData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AccountCategory",
                columns: new[] { "AccountCategoryId", "AccountCategoryName" },
                values: new object[,]
                {
                    { 1, "Asset" },
                    { 2, "Liability" }
                });

            migrationBuilder.InsertData(
                table: "AccountType",
                columns: new[] { "AccountTypeId", "AccountCategoryId", "AccountTypeName" },
                values: new object[,]
                {
                    { 1, 1, "Checking" },
                    { 2, 1, "Savings" },
                    { 3, 1, "Investment" },
                    { 4, 2, "Credit Card" },
                    { 5, 2, "Loan" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AccountType",
                keyColumn: "AccountTypeId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AccountType",
                keyColumn: "AccountTypeId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AccountType",
                keyColumn: "AccountTypeId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "AccountType",
                keyColumn: "AccountTypeId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "AccountType",
                keyColumn: "AccountTypeId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "AccountCategory",
                keyColumn: "AccountCategoryId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AccountCategory",
                keyColumn: "AccountCategoryId",
                keyValue: 2);
        }
    }
}
