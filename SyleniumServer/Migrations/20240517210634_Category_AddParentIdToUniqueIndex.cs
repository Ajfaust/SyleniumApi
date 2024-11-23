using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SyleniumServer.Migrations
{
    /// <inheritdoc />
    public partial class Category_AddParentIdToUniqueIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TransactionCategory_Name",
                table: "TransactionCategory");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionCategory_Name_ParentCategoryId",
                table: "TransactionCategory",
                columns: new[] { "Name", "ParentCategoryId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TransactionCategory_Name_ParentCategoryId",
                table: "TransactionCategory");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionCategory_Name",
                table: "TransactionCategory",
                column: "Name",
                unique: true);
        }
    }
}
