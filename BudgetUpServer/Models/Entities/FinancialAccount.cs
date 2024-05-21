using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BudgetUpServer.Models.Entities
{
    [Table("FinancialAccount")]
    public class FinancialAccount
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FinancialAccountId { get; set; }

        [Required]
        public string Name { get; set; }

        #region FinancialAccountType Relation

        public int FinancialAccountTypeId { get; set; }

        public virtual FinancialAccountType FinancialAccountType { get; set; }

        #endregion FinancialAccountType Relation
    }
}