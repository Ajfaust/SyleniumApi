using BudgetUpServer.Entity;
using Microsoft.EntityFrameworkCore;

namespace BudgetUpServer.Contexts
{
    public class BudgetContext : DbContext
    {
        public DbSet<Portfolio> Portfolios { get; set; }
        public DbSet<FinancialAccount> FinancialAccounts { get; set; }
        public DbSet<FinancialAccountType> FinancialAccountTypes { get; set; }
        public DbSet<FinancialCategory> FinancialCategories { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<TransactionCategory> TransactionCategories { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        public BudgetContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseIdentityColumns();

            modelBuilder.Entity<Portfolio>().HasData(new { PortfolioId = 1, Name = "My Portfolio" });

            modelBuilder.Entity<FinancialCategory>().HasData(
                new { FinancialCategoryId = 1, Name = "Asset" },
                new { FinancialCategoryId = 2, Name = "Liability" });

            modelBuilder.Entity<FinancialAccountType>().HasData(
                new { FinancialAccountTypeId = 1, FinancialCategoryId = 1, Name = "Checking" },
                new { FinancialAccountTypeId = 2, FinancialCategoryId = 1, Name = "Savings" },
                new { FinancialAccountTypeId = 3, FinancialCategoryId = 1, Name = "Investment" },
                new { FinancialAccountTypeId = 4, FinancialCategoryId = 2, Name = "Credit Card" });

            base.OnModelCreating(modelBuilder);
        }
    }
}