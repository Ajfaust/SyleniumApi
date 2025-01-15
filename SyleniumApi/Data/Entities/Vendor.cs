using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SyleniumApi.Data.Entities;

[Table("Vendor")]
public class Vendor
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int VendorId { get; set; }

    [Required]
    [MaxLength(200)]
    public required string VendorName { get; set; }

    #region Transactions Relationship

    public virtual ICollection<Transaction> Transactions { get; } = new List<Transaction>();

    #endregion

    #region Ledger Relationship

    [Required]
    public required int LedgerId { get; set; }

    public virtual Ledger? Ledger { get; set; }

    #endregion
}