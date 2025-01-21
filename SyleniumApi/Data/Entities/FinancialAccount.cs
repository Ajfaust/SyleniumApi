using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SyleniumApi.Data.Entities;

[Table("FinancialAccount")]
public class FinancialAccount
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public required string Name { get; set; }

    #region Transaction Relation

    public virtual ICollection<Transaction> Transactions { get; } = new List<Transaction>();

    #endregion Transaction Relation

    #region Ledger Relation

    [Required]
    public int LedgerId { get; set; }

    public virtual Ledger? Ledger { get; set; }

    #endregion

    #region Financial Account Category Relation

    [Required]
    public int FinancialAccountCategoryId { get; set; }

    public virtual FinancialAccountCategory? FinancialAccountCategory { get; set; }

    #endregion
}