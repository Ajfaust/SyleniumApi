using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BudgetUpServer.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
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
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialCategory", x => x.FinancialCategoryId);
                });

            migrationBuilder.CreateTable(
                name: "Portfolio",
                columns: table => new
                {
                    PortfolioId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Portfolio", x => x.PortfolioId);
                });

            migrationBuilder.CreateTable(
                name: "FinancialAccountType",
                columns: table => new
                {
                    FinancialAccountTypeId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    FinancialCategoryId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialAccountType", x => x.FinancialAccountTypeId);
                    table.ForeignKey(
                        name: "FK_FinancialAccountType_FinancialCategory_FinancialCategoryId",
                        column: x => x.FinancialCategoryId,
                        principalTable: "FinancialCategory",
                        principalColumn: "FinancialCategoryId",
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
                    PortfolioId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionCategory", x => x.TransactionCategoryId);
                    table.ForeignKey(
                        name: "FK_TransactionCategory_Portfolio_PortfolioId",
                        column: x => x.PortfolioId,
                        principalTable: "Portfolio",
                        principalColumn: "PortfolioId",
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
                    PortfolioId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vendor", x => x.VendorId);
                    table.ForeignKey(
                        name: "FK_Vendor_Portfolio_PortfolioId",
                        column: x => x.PortfolioId,
                        principalTable: "Portfolio",
                        principalColumn: "PortfolioId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FinancialAccount",
                columns: table => new
                {
                    FinancialAccountId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    PortfolioId = table.Column<int>(type: "integer", nullable: false),
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
                        name: "FK_FinancialAccount_Portfolio_PortfolioId",
                        column: x => x.PortfolioId,
                        principalTable: "Portfolio",
                        principalColumn: "PortfolioId",
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
                    PortfolioId = table.Column<int>(type: "integer", nullable: false),
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
                        name: "FK_Transaction_Portfolio_PortfolioId",
                        column: x => x.PortfolioId,
                        principalTable: "Portfolio",
                        principalColumn: "PortfolioId",
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

            migrationBuilder.InsertData(
                table: "FinancialCategory",
                columns: new[] { "FinancialCategoryId", "Name" },
                values: new object[,]
                {
                    { 1, "Asset" },
                    { 2, "Liability" }
                });

            migrationBuilder.InsertData(
                table: "Portfolio",
                columns: new[] { "PortfolioId", "Name" },
                values: new object[] { 1, "My Portfolio" });

            migrationBuilder.InsertData(
                table: "FinancialAccountType",
                columns: new[] { "FinancialAccountTypeId", "FinancialCategoryId", "Name" },
                values: new object[,]
                {
                    { 1, 1, "Checking" },
                    { 2, 1, "Savings" },
                    { 3, 1, "Investment" },
                    { 4, 2, "Credit Card" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_FinancialAccount_FinancialAccountTypeId",
                table: "FinancialAccount",
                column: "FinancialAccountTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialAccount_PortfolioId",
                table: "FinancialAccount",
                column: "PortfolioId");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialAccountType_FinancialCategoryId",
                table: "FinancialAccountType",
                column: "FinancialCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_FinancialAccountId",
                table: "Transaction",
                column: "FinancialAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_PortfolioId",
                table: "Transaction",
                column: "PortfolioId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_TransactionCategoryId",
                table: "Transaction",
                column: "TransactionCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_VendorId",
                table: "Transaction",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionCategory_ParentCategoryTransactionCategoryId",
                table: "TransactionCategory",
                column: "ParentCategoryTransactionCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionCategory_PortfolioId",
                table: "TransactionCategory",
                column: "PortfolioId");

            migrationBuilder.CreateIndex(
                name: "IX_Vendor_PortfolioId",
                table: "Vendor",
                column: "PortfolioId");
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
                name: "Portfolio");

            migrationBuilder.DropTable(
                name: "FinancialCategory");
        }
    }
}
