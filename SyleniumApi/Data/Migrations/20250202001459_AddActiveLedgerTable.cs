using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SyleniumApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddActiveLedgerTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Ledger");

            migrationBuilder.CreateTable(
                name: "ActiveLedger",
                columns: table => new
                {
                    LedgerId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActiveLedger", x => x.LedgerId);
                    table.ForeignKey(
                        name: "FK_ActiveLedger_Ledger_LedgerId",
                        column: x => x.LedgerId,
                        principalTable: "Ledger",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActiveLedger");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Ledger",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
