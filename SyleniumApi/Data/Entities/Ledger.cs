using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SyleniumApi.Data.Entities;

[Table("Ledger")]
public class Ledger
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int LedgerId { get; set; }

    [Required]
    [MaxLength(200)]
    public required string LedgerName { get; set; }

    [Required]
    public required DateTime CreatedDate { get; set; }

    #region Financial Account Category Relation

    public virtual ICollection<FinancialAccountCategory> FinancialAccountCategories { get; set; } =
        new List<FinancialAccountCategory>();

    #endregion

    #region Account Relation

    public virtual ICollection<FinancialAccount> FinancialAccounts { get; set; } = new List<FinancialAccount>();

    #endregion
}