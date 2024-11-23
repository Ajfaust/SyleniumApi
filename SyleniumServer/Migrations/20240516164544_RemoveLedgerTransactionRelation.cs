using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SyleniumServer.Migrations
{
    /// <inheritdoc />
    public partial class RemoveLedgerTransactionRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Ledger_LedgerId",
                table: "Transaction");

            migrationBuilder.DropIndex(
                name: "IX_Transaction_LedgerId",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "LedgerId",
                table: "Transaction");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LedgerId",
                table: "Transaction",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_LedgerId",
                table: "Transaction",
                column: "LedgerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Ledger_LedgerId",
                table: "Transaction",
                column: "LedgerId",
                principalTable: "Ledger",
                principalColumn: "LedgerId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
