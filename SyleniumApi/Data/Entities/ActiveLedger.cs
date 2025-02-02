using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SyleniumApi.Data.Entities;

[Table("ActiveLedger")]
public class ActiveLedger
{
    [Key]
    public int LedgerId { get; set; }

    [ForeignKey("LedgerId")]
    public Ledger? Ledger { get; set; }
}