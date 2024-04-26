using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BudgetUpServer.Entity
{
    public class AccountType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AccountTypeId { get; set; }

        [Required]
        public string AccountTypeName { get; set; }

        #region AccountCategory Relation

        public int AccountCategoryId { get; set; }
        public virtual AccountCategory AccountCategory { get; set; }

        #endregion AccountCategory Relation

        #region Account Relation

        public virtual ICollection<Account> Accounts { get; set; }

        #endregion Account Relation
    }
}