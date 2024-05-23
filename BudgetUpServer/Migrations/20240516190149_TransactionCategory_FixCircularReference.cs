using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AllostaServer.Migrations
{
    /// <inheritdoc />
    public partial class TransactionCategory_FixCircularReference : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransactionCategory_TransactionCategory_ParentCategoryTrans~",
                table: "TransactionCategory");

            migrationBuilder.DropIndex(
                name: "IX_TransactionCategory_ParentCategoryTransactionCategoryId",
                table: "TransactionCategory");

            migrationBuilder.DropColumn(
                name: "ParentCategoryTransactionCategoryId",
                table: "TransactionCategory");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "TransactionCategory");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParentCategoryTransactionCategoryId",
                table: "TransactionCategory",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "TransactionCategory",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TransactionCategory_ParentCategoryTransactionCategoryId",
                table: "TransactionCategory",
                column: "ParentCategoryTransactionCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionCategory_TransactionCategory_ParentCategoryTrans~",
                table: "TransactionCategory",
                column: "ParentCategoryTransactionCategoryId",
                principalTable: "TransactionCategory",
                principalColumn: "TransactionCategoryId");
        }
    }
}
