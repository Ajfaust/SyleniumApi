using BudgetUpServer.Entity;
using Microsoft.EntityFrameworkCore;

namespace BudgetUpServer.Contexts
{
    public class BudgetContext : DbContext
    {
        public DbSet<Spreadsheet> Spreadsheets { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountType> AccountTypes { get; set; }
        public DbSet<AccountCategory> AccountCategories { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<TransactionCategory> TransactionCategories { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        public BudgetContext(DbContextOptions options) : base(options)
        {
        }
    }
}