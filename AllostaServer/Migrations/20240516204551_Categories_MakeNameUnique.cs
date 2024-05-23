using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AllostaServer.Migrations
{
    /// <inheritdoc />
    public partial class Categories_MakeNameUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_TransactionCategory_Name",
                table: "TransactionCategory",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TransactionCategory_Name",
                table: "TransactionCategory");
        }
    }
}
