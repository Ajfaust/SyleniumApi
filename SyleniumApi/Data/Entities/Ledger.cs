using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SyleniumApi.Models.Entities;

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
}