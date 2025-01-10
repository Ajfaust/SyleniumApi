using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SyleniumApi.Data.Entities;

[Table("FinancialAccountCategory")]
public class FinancialAccountCategory
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int FinancialAccountCategoryId { get; set; }
    
    [Required]
    [MaxLength(200)]
    public required string FinancialAccountCategoryName { get; set; }
    
    [Required]
    public required FinancialCategoryType FinancialCategoryType { get; set; }
}

public enum FinancialCategoryType
{
    Asset,
    Liability
}