using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SyleniumApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class Ledger_ToFluentApi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Ledger_Id",
                table: "Ledger",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Ledger_Id",
                table: "Ledger");
        }
    }
}
