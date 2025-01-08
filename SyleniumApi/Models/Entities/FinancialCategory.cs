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
    
    #region Journal Relation
    
    public int JournalId { get; set; }
    
    public virtual required Journal Journal { get; set; }
    
    #endregion
}