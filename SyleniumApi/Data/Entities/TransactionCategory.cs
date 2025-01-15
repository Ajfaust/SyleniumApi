using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SyleniumApi.Data.Entities;

/// <summary>
///     TransactionCategory class
/// </summary>
[Table("TransactionCategory")]
public class TransactionCategory
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [ForeignKey(nameof(ParentCategoryId))]
    public int TransactionCategoryId { get; set; }

    [Required]
    [MaxLength(200)]
    public required string TransactionCategoryName { get; set; }

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