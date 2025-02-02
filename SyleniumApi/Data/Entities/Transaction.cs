using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SyleniumApi.Data.Entities;

[EntityTypeConfiguration(typeof(TransactionConfiguration))]
public class Transaction
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public DateTime Date { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }

    public decimal Inflow { get; set; }

    public decimal Outflow { get; set; }

    public bool Cleared { get; set; }

    #region Vendor Relation

    public int VendorId { get; set; }

    public virtual Vendor? Vendor { get; set; }

    #endregion Vendor Relation

    #region Transaction Category Relation

    public int TransactionCategoryId { get; set; }

    public virtual TransactionCategory? TransactionCategory { get; set; }

    #endregion Transaction Category Relation

    #region Financial Account Relation

    [Required]
    public required int FinancialAccountId { get; set; }

    public virtual FinancialAccount? FinancialAccount { get; set; }

    #endregion Financial Account Relation
}

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable("Transactions")
            .HasKey(e => e.Id);

        builder.Property(e => e.Date).IsRequired();

        builder.HasOne<FinancialAccount>(e => e.FinancialAccount)
            .WithMany(fa => fa.Transactions)
            .HasForeignKey(e => e.TransactionCategoryId)
            .HasPrincipalKey(fa => fa.Id);
        builder.HasOne<TransactionCategory>(e => e.TransactionCategory)
            .WithMany(tc => tc.Transactions)
            .HasForeignKey(e => e.TransactionCategoryId)
            .HasPrincipalKey(tc => tc.Id);
        builder.HasOne<Vendor>(e => e.Vendor)
            .WithMany(v => v.Transactions)
            .HasForeignKey(e => e.VendorId)
            .HasPrincipalKey(v => v.Id);
    }
}