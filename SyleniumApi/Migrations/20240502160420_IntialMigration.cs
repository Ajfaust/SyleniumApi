using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SyleniumApi.Migrations
{
    /// <inheritdoc />
    public partial class IntialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FinancialAccountType",
                columns: table => new
                {
                    FinancialAccountTypeId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    FinancialCategory = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialAccountType", x => x.FinancialAccountTypeId);
                });

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

            migrationBuilder.CreateTable(
                name: "FinancialAccount",
                columns: table => new
                {
                    FinancialAccountId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    LedgerId = table.Column<int>(type: "integer", nullable: false),
                    FinancialAccountTypeId = table.Column<int>(type: "integer", nullable: false)
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
                    table.ForeignKey(
                        name: "FK_FinancialAccount_Ledger_LedgerId",
                        column: x => x.LedgerId,
                        principalTable: "Ledger",
                        principalColumn: "LedgerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TransactionCategory",
                columns: table => new
                {
                    TransactionCategoryId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ParentId = table.Column<int>(type: "integer", nullable: true),
                    ParentCategoryTransactionCategoryId = table.Column<int>(type: "integer", nullable: true),
                    LedgerId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionCategory", x => x.TransactionCategoryId);
                    table.ForeignKey(
                        name: "FK_TransactionCategory_Ledger_LedgerId",
                        column: x => x.LedgerId,
                        principalTable: "Ledger",
                        principalColumn: "LedgerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransactionCategory_TransactionCategory_ParentCategoryTrans~",
                        column: x => x.ParentCategoryTransactionCategoryId,
                        principalTable: "TransactionCategory",
                        principalColumn: "TransactionCategoryId");
                });

            migrationBuilder.CreateTable(
                name: "Vendor",
                columns: table => new
                {
                    VendorId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    LedgerId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vendor", x => x.VendorId);
                    table.ForeignKey(
                        name: "FK_Vendor_Ledger_LedgerId",
                        column: x => x.LedgerId,
                        principalTable: "Ledger",
                        principalColumn: "LedgerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transaction",
                columns: table => new
                {
                    TransactionId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    Inflow = table.Column<decimal>(type: "numeric", nullable: false),
                    Outflow = table.Column<decimal>(type: "numeric", nullable: false),
                    Cleared = table.Column<bool>(type: "boolean", nullable: false),
                    LedgerId = table.Column<int>(type: "integer", nullable: false),
                    VendorId = table.Column<int>(type: "integer", nullable: true),
                    TransactionCategoryId = table.Column<int>(type: "integer", nullable: true),
                    FinancialAccountId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transaction", x => x.TransactionId);
                    table.ForeignKey(
                        name: "FK_Transaction_FinancialAccount_FinancialAccountId",
                        column: x => x.FinancialAccountId,
                        principalTable: "FinancialAccount",
                        principalColumn: "FinancialAccountId");
                    table.ForeignKey(
                        name: "FK_Transaction_Ledger_LedgerId",
                        column: x => x.LedgerId,
                        principalTable: "Ledger",
                        principalColumn: "LedgerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transaction_TransactionCategory_TransactionCategoryId",
                        column: x => x.TransactionCategoryId,
                        principalTable: "TransactionCategory",
                        principalColumn: "TransactionCategoryId");
                    table.ForeignKey(
                        name: "FK_Transaction_Vendor_VendorId",
                        column: x => x.VendorId,
                        principalTable: "Vendor",
                        principalColumn: "VendorId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FinancialAccount_FinancialAccountTypeId",
                table: "FinancialAccount",
                column: "FinancialAccountTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialAccount_LedgerId",
                table: "FinancialAccount",
                column: "LedgerId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_FinancialAccountId",
                table: "Transaction",
                column: "FinancialAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_LedgerId",
                table: "Transaction",
                column: "LedgerId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_TransactionCategoryId",
                table: "Transaction",
                column: "TransactionCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_VendorId",
                table: "Transaction",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionCategory_LedgerId",
                table: "TransactionCategory",
                column: "LedgerId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionCategory_ParentCategoryTransactionCategoryId",
                table: "TransactionCategory",
                column: "ParentCategoryTransactionCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Vendor_LedgerId",
                table: "Vendor",
                column: "LedgerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transaction");

            migrationBuilder.DropTable(
                name: "FinancialAccount");

            migrationBuilder.DropTable(
                name: "TransactionCategory");

            migrationBuilder.DropTable(
                name: "Vendor");

            migrationBuilder.DropTable(
                name: "FinancialAccountType");

            migrationBuilder.DropTable(
                name: "Ledger");
        }
    }
}