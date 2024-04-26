using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BudgetUpServer.Entity
{
    public class AccountCategory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AccountCategoryId { get; set; }

        [Required]
        public string AccountCategoryName { get; set; }

        #region AccountType Relation

        public virtual ICollection<AccountType> AccountTypes { get; set; }

        #endregion AccountType Relation
    }
}