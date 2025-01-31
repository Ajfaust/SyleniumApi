using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SyleniumApi.Data.Entities;

[Table("Ledger")]
public class Ledger
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public required string Name { get; set; }

    public DateTime CreatedDate { get; set; }

    public bool IsActive { get; set; }

    #region Financial Account Category Relation

    public virtual ICollection<FinancialAccountCategory> FinancialAccountCategories { get; set; } =
        new List<FinancialAccountCategory>();

    #endregion

    #region Account Relation

    public virtual ICollection<FinancialAccount> FinancialAccounts { get; set; } = new List<FinancialAccount>();

    #endregion
}