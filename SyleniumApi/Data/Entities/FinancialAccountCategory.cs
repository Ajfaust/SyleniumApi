using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SyleniumApi.Data.Entities;

[Table("FinancialAccountCategory")]
public class FinancialAccountCategory
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public required string Name { get; set; }

    [Required]
    public required FinancialCategoryType Type { get; set; }

    #region Ledger Relation

    public required int LedgerId { get; set; }

    public virtual Ledger? Ledger { get; set; }

    #endregion
}

public enum FinancialCategoryType
{
    Asset,
    Liability
}