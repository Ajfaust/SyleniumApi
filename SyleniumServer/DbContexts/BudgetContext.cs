using Microsoft.EntityFrameworkCore;
using SyleniumServer.Models.Entities;

namespace SyleniumServer.DbContexts
{
    public class BudgetContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<TransactionCategory> TransactionCategories { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        public BudgetContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseIdentityColumns();

            modelBuilder.Entity<TransactionCategory>()
                .HasMany(e => e.SubCategories)
                .WithOne(e => e.ParentCategory)
                .HasForeignKey(e => e.ParentCategoryId);

            modelBuilder.Entity<TransactionCategory>().HasIndex(e => new { e.Name, e.ParentCategoryId }).IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}