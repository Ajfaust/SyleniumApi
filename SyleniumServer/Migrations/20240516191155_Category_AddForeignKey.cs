using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SyleniumServer.Migrations
{
    /// <inheritdoc />
    public partial class Category_AddForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParentCategoryId",
                table: "TransactionCategory",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TransactionCategory_ParentCategoryId",
                table: "TransactionCategory",
                column: "ParentCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionCategory_TransactionCategory_ParentCategoryId",
                table: "TransactionCategory",
                column: "ParentCategoryId",
                principalTable: "TransactionCategory",
                principalColumn: "TransactionCategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransactionCategory_TransactionCategory_ParentCategoryId",
                table: "TransactionCategory");

            migrationBuilder.DropIndex(
                name: "IX_TransactionCategory_ParentCategoryId",
                table: "TransactionCategory");

            migrationBuilder.DropColumn(
                name: "ParentCategoryId",
                table: "TransactionCategory");
        }
    }
}
