using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SyleniumApi.Data.Entities;

[EntityTypeConfiguration(typeof(VendorConfiguration))]
public class Vendor
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [MaxLength(200)]
    public required string Name { get; set; }

    #region Transactions Relationship

    public virtual ICollection<Transaction> Transactions { get; } = new List<Transaction>();

    #endregion

    #region Ledger Relationship

    [Required]
    public required int LedgerId { get; set; }

    public virtual Ledger? Ledger { get; set; }

    #endregion
}

public class VendorConfiguration : IEntityTypeConfiguration<Vendor>
{
    public void Configure(EntityTypeBuilder<Vendor> builder)
    {
        builder.ToTable("Vendors").HasKey(e => e.Id);

        builder.Property(e => e.Name).IsRequired();

        builder.HasOne<Ledger>(e => e.Ledger)
            .WithMany(l => l.Vendors)
            .HasForeignKey(e => e.LedgerId)
            .IsRequired()
            .HasPrincipalKey(l => l.Id);
    }
}