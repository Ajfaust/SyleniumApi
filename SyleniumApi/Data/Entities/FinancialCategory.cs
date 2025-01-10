using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SyleniumApi.Models.Entities;

[Table("FinancialCategory")]
public class FinancialCategory
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int FinancialCategoryId { get; set; }
    
    [Required]
    [MaxLength(200)]
    public required string FinancialCategoryName { get; set; }
}