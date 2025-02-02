using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SyleniumApi.Data.Entities;

[EntityTypeConfiguration(typeof(FinancialAccountConfiguration))]
public class FinancialAccount
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [MaxLength(200)]
    public required string Name { get; set; }

    #region Transaction Relation

    public virtual ICollection<Transaction> Transactions { get; } = new List<Transaction>();

    #endregion Transaction Relation

    #region Ledger Relation

    public int LedgerId { get; set; }

    public virtual Ledger? Ledger { get; set; }

    #endregion

    #region Financial Account Category Relation

    public int FinancialAccountCategoryId { get; set; }

    public virtual FinancialAccountCategory? FinancialAccountCategory { get; set; }

    #endregion
}

public class FinancialAccountConfiguration : IEntityTypeConfiguration<FinancialAccount>
{
    public void Configure(EntityTypeBuilder<FinancialAccount> builder)
    {
        builder.ToTable("FinancialAccounts")
            .HasKey(e => e.Id);

        builder.Property(e => e.Name).IsRequired();

        builder.HasOne(e => e.Ledger)
            .WithMany(l => l.FinancialAccounts)
            .HasForeignKey(e => e.LedgerId)
            .IsRequired()
            .HasPrincipalKey(l => l.Id);
        builder.HasOne(e => e.FinancialAccountCategory)
            .WithMany(fac => fac.FinancialAccounts)
            .HasForeignKey(fa => fa.FinancialAccountCategoryId)
            .IsRequired()
            .HasPrincipalKey(fac => fac.Id);
    }
}