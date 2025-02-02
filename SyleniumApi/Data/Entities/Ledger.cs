using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SyleniumApi.Data.Entities;

[EntityTypeConfiguration(typeof(LedgerConfiguration))]
public class Ledger
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [MaxLength(200)]
    public required string Name { get; set; }

    public DateTime CreatedDate { get; set; }

    #region Financial Account Category Relation

    public virtual ICollection<FinancialAccountCategory> FinancialAccountCategories { get; set; } =
        new List<FinancialAccountCategory>();

    #endregion

    #region Transaction Category Relation

    public virtual ICollection<TransactionCategory> TransactionCategories { get; set; } =
        new List<TransactionCategory>();

    #endregion

    #region Account Relation

    public virtual ICollection<FinancialAccount> FinancialAccounts { get; set; } = new List<FinancialAccount>();

    #endregion

    #region Vendor Relation

    public virtual ICollection<Vendor> Vendors { get; set; } = new List<Vendor>();

    #endregion
}

public class LedgerConfiguration : IEntityTypeConfiguration<Ledger>
{
    public void Configure(EntityTypeBuilder<Ledger> builder)
    {
        builder.ToTable("Ledger");

        builder.HasKey(l => l.Id);
        builder.HasIndex(l => l.Id);
    }
}