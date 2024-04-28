using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BudgetUpServer.Entity
{
    [Table("FinancialCategory")]
    public class FinancialCategory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FinancialCategoryId { get; set; }

        [Required]
        public string Name { get; set; }

        #region FinancialAccountType Relation

        public ICollection<FinancialAccountType> FinancialAccountTypes { get; set; }

        #endregion FinancialAccountType Relation
    }
}