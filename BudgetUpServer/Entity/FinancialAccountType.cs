using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BudgetUpServer.Entity
{
    [Table("FinancialAccountType")]
    public class FinancialAccountType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FinancialAccountTypeId { get; set; }

        [Required]
        public string Name { get; set; }

        #region FinancialCategory Relation

        public int FinancialCategoryId { get; set; }

        public FinancialCategory FinancialCategory { get; set; }

        #endregion FinancialCategory Relation

        #region FinancialAccount Relation

        public virtual ICollection<FinancialAccount> FinancialAccounts { get; set; }

        #endregion FinancialAccount Relation
    }
}