using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AllostaServer.Migrations
{
    /// <inheritdoc />
    public partial class FinancialAccount_RenameToAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_FinancialAccount_FinancialAccountId",
                table: "Transaction");

            migrationBuilder.DropTable(
                name: "FinancialAccount");

            migrationBuilder.DropTable(
                name: "FinancialAccountType");

            migrationBuilder.RenameColumn(
                name: "FinancialAccountId",
                table: "Transaction",
                newName: "AccountId");

            migrationBuilder.RenameIndex(
                name: "IX_Transaction_FinancialAccountId",
                table: "Transaction",
                newName: "IX_Transaction_AccountId");

            migrationBuilder.CreateTable(
                name: "AccountType",
                columns: table => new
                {
                    AccountTypeId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    FinancialCategory = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountType", x => x.AccountTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    AccountId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Balance = table.Column<double>(type: "double precision", nullable: false),
                    AccountTypeId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.AccountId);
                    table.ForeignKey(
                        name: "FK_Account_AccountType_AccountTypeId",
                        column: x => x.AccountTypeId,
                        principalTable: "AccountType",
                        principalColumn: "AccountTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Account_AccountTypeId",
                table: "Account",
                column: "AccountTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Account_AccountId",
                table: "Transaction",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "AccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Account_AccountId",
                table: "Transaction");

            migrationBuilder.DropTable(
                name: "Account");

            migrationBuilder.DropTable(
                name: "AccountType");

            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "Transaction",
                newName: "FinancialAccountId");

            migrationBuilder.RenameIndex(
                name: "IX_Transaction_AccountId",
                table: "Transaction",
                newName: "IX_Transaction_FinancialAccountId");

            migrationBuilder.CreateTable(
                name: "FinancialAccountType",
                columns: table => new
                {
                    FinancialAccountTypeId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FinancialCategory = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialAccountType", x => x.FinancialAccountTypeId);
                });

            migrationBuilder.CreateTable(
                name: "FinancialAccount",
                columns: table => new
                {
                    FinancialAccountId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FinancialAccountTypeId = table.Column<int>(type: "integer", nullable: false),
                    Balance = table.Column<double>(type: "double precision", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialAccount", x => x.FinancialAccountId);
                    table.ForeignKey(
                        name: "FK_FinancialAccount_FinancialAccountType_FinancialAccountTypeId",
                        column: x => x.FinancialAccountTypeId,
                        principalTable: "FinancialAccountType",
                        principalColumn: "FinancialAccountTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FinancialAccount_FinancialAccountTypeId",
                table: "FinancialAccount",
                column: "FinancialAccountTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_FinancialAccount_FinancialAccountId",
                table: "Transaction",
                column: "FinancialAccountId",
                principalTable: "FinancialAccount",
                principalColumn: "FinancialAccountId");
        }
    }
}
