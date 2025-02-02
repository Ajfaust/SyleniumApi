using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SyleniumApi.Data.Entities;

[EntityTypeConfiguration(typeof(ActiveLedgerConfiguration))]
public class ActiveLedger
{
    public int LedgerId { get; set; }

    public Ledger? Ledger { get; set; }
}

public class ActiveLedgerConfiguration : IEntityTypeConfiguration<ActiveLedger>
{
    public void Configure(EntityTypeBuilder<ActiveLedger> builder)
    {
        builder.ToTable("ActiveLedger").HasKey(e => e.LedgerId);

        builder.HasOne<Ledger>(e => e.Ledger)
            .WithOne();
    }
}