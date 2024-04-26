using BudgetUpServer.Entity;
using Microsoft.EntityFrameworkCore;

namespace BudgetUpServer.Contexts
{
    public class BudgetContext : DbContext
    {
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountType> AccountTypes { get; set; }
        public DbSet<AccountCategory> AccountCategories { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<TransactionCategory> TransactionCategories { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        public BudgetContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccountCategory>().HasData(
                new { AccountCategoryId = 1, AccountCategoryName = "Asset" },
                new { AccountCategoryId = 2, AccountCategoryName = "Liability" });
            modelBuilder.Entity<AccountType>().HasData(
                new { AccountTypeId = 1, AccountCategoryId = 1, AccountTypeName = "Checking" },
                new { AccountTypeId = 2, AccountCategoryId = 1, AccountTypeName = "Savings" },
                new { AccountTypeId = 3, AccountCategoryId = 1, AccountTypeName = "Investment" },
                new { AccountTypeId = 4, AccountCategoryId = 2, AccountTypeName = "Credit Card" },
                new { AccountTypeId = 5, AccountCategoryId = 2, AccountTypeName = "Loan" });
            base.OnModelCreating(modelBuilder);
        }
    }
}