using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SyleniumApi.Migrations
{
    /// <inheritdoc />
    public partial class FinancialAccount_AddBalance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Balance",
                table: "FinancialAccount",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Balance",
                table: "FinancialAccount");
        }
    }
}
