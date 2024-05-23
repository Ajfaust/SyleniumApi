using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AllostaServer.Migrations
{
    /// <inheritdoc />
    public partial class DeleteLedger : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FinancialAccount_Ledger_LedgerId",
                table: "FinancialAccount");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionCategory_Ledger_LedgerId",
                table: "TransactionCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_Vendor_Ledger_LedgerId",
                table: "Vendor");

            migrationBuilder.DropTable(
                name: "Ledger");

            migrationBuilder.DropIndex(
                name: "IX_Vendor_LedgerId",
                table: "Vendor");

            migrationBuilder.DropIndex(
                name: "IX_TransactionCategory_LedgerId",
                table: "TransactionCategory");

            migrationBuilder.DropIndex(
                name: "IX_FinancialAccount_LedgerId",
                table: "FinancialAccount");

            migrationBuilder.DropColumn(
                name: "LedgerId",
                table: "Vendor");

            migrationBuilder.DropColumn(
                name: "LedgerId",
                table: "TransactionCategory");

            migrationBuilder.DropColumn(
                name: "LedgerId",
                table: "FinancialAccount");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LedgerId",
                table: "Vendor",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LedgerId",
                table: "TransactionCategory",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LedgerId",
                table: "FinancialAccount",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Ledger",
                columns: table => new
                {
                    LedgerId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ledger", x => x.LedgerId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Vendor_LedgerId",
                table: "Vendor",
                column: "LedgerId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionCategory_LedgerId",
                table: "TransactionCategory",
                column: "LedgerId");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialAccount_LedgerId",
                table: "FinancialAccount",
                column: "LedgerId");

            migrationBuilder.AddForeignKey(
                name: "FK_FinancialAccount_Ledger_LedgerId",
                table: "FinancialAccount",
                column: "LedgerId",
                principalTable: "Ledger",
                principalColumn: "LedgerId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionCategory_Ledger_LedgerId",
                table: "TransactionCategory",
                column: "LedgerId",
                principalTable: "Ledger",
                principalColumn: "LedgerId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Vendor_Ledger_LedgerId",
                table: "Vendor",
                column: "LedgerId",
                principalTable: "Ledger",
                principalColumn: "LedgerId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
