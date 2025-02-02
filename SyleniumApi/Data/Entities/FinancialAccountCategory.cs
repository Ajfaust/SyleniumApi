using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SyleniumApi.Data.Entities;

[EntityTypeConfiguration(typeof(FinancialAccountCategoryConfiguration))]
public class FinancialAccountCategory
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [MaxLength(200)]
    public required string Name { get; set; }

    public required FinancialCategoryType Type { get; set; }

    #region Financial Account Relation

    public virtual ICollection<FinancialAccount> FinancialAccounts { get; set; } = new List<FinancialAccount>();

    #endregion

    #region Ledger Relation

    public int LedgerId { get; set; }

    public virtual Ledger? Ledger { get; set; }

    #endregion
}

public enum FinancialCategoryType
{
    Asset,
    Liability
}

public class FinancialAccountCategoryConfiguration : IEntityTypeConfiguration<FinancialAccountCategory>
{
    public void Configure(EntityTypeBuilder<FinancialAccountCategory> builder)
    {
        builder.ToTable("FinancialAccountCategories")
            .HasKey(e => e.Id);

        builder.Property(e => e.Name).IsRequired();
        builder.Property(e => e.Type).IsRequired();

        builder.HasOne(e => e.Ledger)
            .WithMany(l => l.FinancialAccountCategories)
            .HasForeignKey(e => e.LedgerId)
            .IsRequired()
            .HasPrincipalKey(l => l.Id);
    }
}