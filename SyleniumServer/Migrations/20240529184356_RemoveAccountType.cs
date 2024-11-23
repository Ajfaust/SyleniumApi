using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SyleniumServer.Migrations
{
    /// <inheritdoc />
    public partial class RemoveAccountType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Account_AccountType_AccountTypeId",
                table: "Account");

            migrationBuilder.DropTable(
                name: "AccountType");

            migrationBuilder.DropIndex(
                name: "IX_Account_AccountTypeId",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "AccountTypeId",
                table: "Account");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AccountTypeId",
                table: "Account",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AccountType",
                columns: table => new
                {
                    AccountTypeId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FinancialCategory = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountType", x => x.AccountTypeId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Account_AccountTypeId",
                table: "Account",
                column: "AccountTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Account_AccountType_AccountTypeId",
                table: "Account",
                column: "AccountTypeId",
                principalTable: "AccountType",
                principalColumn: "AccountTypeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
