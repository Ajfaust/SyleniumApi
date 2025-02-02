using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SyleniumApi.Data.Entities;

/// <summary>
///     TransactionCategory class
/// </summary>
[EntityTypeConfiguration(typeof(TransactionCategoryConfiguration))]
public class TransactionCategory
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [MaxLength(200)]
    public required string Name { get; set; }

    #region Transaction Relation

    public virtual ICollection<Transaction>? Transactions { get; set; }

    #endregion Transaction Relation

    #region Ledger Relation

    [Required]
    public required int LedgerId { get; set; }

    public virtual Ledger? Ledger { get; set; }

    #endregion

    #region Parent-Child Relation

    public int? ParentCategoryId { get; set; }

    public virtual TransactionCategory? ParentCategory { get; set; }

    public virtual ICollection<TransactionCategory> SubCategories { get; } = new List<TransactionCategory>();

    #endregion Parent-Child Relation
}

public class TransactionCategoryConfiguration : IEntityTypeConfiguration<TransactionCategory>
{
    public void Configure(EntityTypeBuilder<TransactionCategory> builder)
    {
        builder.ToTable("TransactionCategories")
            .HasKey(e => e.Id);

        builder.Property(e => e.Name).IsRequired();

        builder.HasOne<Ledger>(e => e.Ledger)
            .WithMany(l => l.TransactionCategories)
            .HasForeignKey(e => e.LedgerId)
            .IsRequired()
            .HasPrincipalKey(l => l.Id);

        builder.HasOne<TransactionCategory>(e => e.ParentCategory)
            .WithMany(tc => tc.SubCategories)
            .HasForeignKey(e => e.ParentCategoryId)
            .HasPrincipalKey(tc => tc.Id);

        builder.HasIndex(e => new { e.Name, e.ParentCategoryId }).IsUnique();
    }
}