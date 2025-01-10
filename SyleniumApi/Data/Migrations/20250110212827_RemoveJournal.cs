using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SyleniumApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveJournal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FinancialCategory_Journal_JournalId",
                table: "FinancialCategory");

            migrationBuilder.DropForeignKey(
                name: "FK_Ledger_Journal_JournalId",
                table: "Ledger");

            migrationBuilder.DropTable(
                name: "Journal");

            migrationBuilder.DropIndex(
                name: "IX_Ledger_JournalId",
                table: "Ledger");

            migrationBuilder.DropIndex(
                name: "IX_FinancialCategory_JournalId",
                table: "FinancialCategory");

            migrationBuilder.DropColumn(
                name: "JournalId",
                table: "Ledger");

            migrationBuilder.DropColumn(
                name: "JournalId",
                table: "FinancialCategory");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "JournalId",
                table: "Ledger",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "JournalId",
                table: "FinancialCategory",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Journal",
                columns: table => new
                {
                    JournalId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    JournalName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Journal", x => x.JournalId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ledger_JournalId",
                table: "Ledger",
                column: "JournalId");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialCategory_JournalId",
                table: "FinancialCategory",
                column: "JournalId");

            migrationBuilder.AddForeignKey(
                name: "FK_FinancialCategory_Journal_JournalId",
                table: "FinancialCategory",
                column: "JournalId",
                principalTable: "Journal",
                principalColumn: "JournalId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ledger_Journal_JournalId",
                table: "Ledger",
                column: "JournalId",
                principalTable: "Journal",
                principalColumn: "JournalId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
