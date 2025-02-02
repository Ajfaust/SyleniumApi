using Microsoft.EntityFrameworkCore;
using SyleniumApi.Data.Entities;

namespace SyleniumApi.DbContexts;

/// <summary>
///     Context for a Budget
/// </summary>
public class SyleniumDbContext(DbContextOptions options) : DbContext(options)
{
    /// <summary>
    ///     DB Set for Ledgers
    /// </summary>
    public DbSet<Ledger> Ledgers { get; set; }

    /// <summary>
    ///     DB Set for Active Ledger
    /// </summary>
    public DbSet<ActiveLedger> ActiveLedger { get; set; }

    /// <summary>
    ///     DB Set for Financial Categories
    /// </summary>
    public DbSet<FinancialAccountCategory> FinancialAccountCategories { get; set; }

    /// <summary>
    ///     DB Set for Accounts
    /// </summary>
    public DbSet<FinancialAccount> FinancialAccounts { get; set; }

    /// <summary>
    ///     DB Set for Vendors
    /// </summary>
    public DbSet<Vendor> Vendors { get; set; }

    /// <summary>
    ///     DB Set for TransactionCategories
    /// </summary>
    public DbSet<TransactionCategory> TransactionCategories { get; set; }

    /// <summary>
    ///     DB Set for Transactions
    /// </summary>
    public DbSet<Transaction> Transactions { get; set; }
}