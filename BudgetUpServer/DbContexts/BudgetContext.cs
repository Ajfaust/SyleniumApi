using BudgetUpServer.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BudgetUpServer.DbContexts
{
    public class BudgetContext : DbContext
    {
        public DbSet<Ledger> Ledgers { get; set; }
        public DbSet<FinancialAccount> FinancialAccounts { get; set; }
        public DbSet<FinancialAccountType> FinancialAccountTypes { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<TransactionCategory> TransactionCategories { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        public BudgetContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseIdentityColumns();

            base.OnModelCreating(modelBuilder);
        }
    }
}