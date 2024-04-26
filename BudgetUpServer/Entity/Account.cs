using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BudgetUpServer.Entity
{
    public class Account
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AccountId { get; set; }

        [Required]
        public string AccountName { get; set; }

        #region Profile Relation

        public int ProfileId { get; set; }

        public virtual Profile Profile { get; set; }

        #endregion Profile Relation

        #region AccountType Relation

        public int AccountTypeId { get; set; }

        public virtual AccountType AccountType { get; set; }

        #endregion AccountType Relation
    }
}