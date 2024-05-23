using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AllostaServer.Migrations
{
    /// <inheritdoc />
    public partial class Transaction_Category_MakeRelationshipRequired : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_TransactionCategory_TransactionCategoryId",
                table: "Transaction");

            migrationBuilder.AlterColumn<int>(
                name: "TransactionCategoryId",
                table: "Transaction",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_TransactionCategory_TransactionCategoryId",
                table: "Transaction",
                column: "TransactionCategoryId",
                principalTable: "TransactionCategory",
                principalColumn: "TransactionCategoryId",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_TransactionCategory_TransactionCategoryId",
                table: "Transaction");

            migrationBuilder.AlterColumn<int>(
                name: "TransactionCategoryId",
                table: "Transaction",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_TransactionCategory_TransactionCategoryId",
                table: "Transaction",
                column: "TransactionCategoryId",
                principalTable: "TransactionCategory",
                principalColumn: "TransactionCategoryId");
        }
    }
}
