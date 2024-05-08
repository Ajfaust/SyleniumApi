using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BudgetUpServer.Models.Entities
{
    public enum FinancialCategory
    {
        Asset = 0,
        Liablility = 1
    }

    [Table("FinancialAccountType")]
    public class FinancialAccountType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FinancialAccountTypeId { get; set; }

        [Required]
        public string Name { get; set; }

        public FinancialCategory FinancialCategory { get; set; }

        #region FinancialAccount Relation

        public virtual ICollection<FinancialAccount> FinancialAccounts { get; set; }

        #endregion FinancialAccount Relation
    }
}