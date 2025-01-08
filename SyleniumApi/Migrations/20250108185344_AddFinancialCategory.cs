using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SyleniumApi.Migrations
{
    /// <inheritdoc />
    public partial class AddFinancialCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FinancialCategory",
                columns: table => new
                {
                    FinancialCategoryId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FinancialCategoryName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    JournalId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialCategory", x => x.FinancialCategoryId);
                    table.ForeignKey(
                        name: "FK_FinancialCategory_Journal_JournalId",
                        column: x => x.JournalId,
                        principalTable: "Journal",
                        principalColumn: "JournalId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FinancialCategory_JournalId",
                table: "FinancialCategory",
                column: "JournalId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FinancialCategory");
        }
    }
}
