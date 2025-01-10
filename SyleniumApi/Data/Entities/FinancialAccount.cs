using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SyleniumApi.Data.Entities
{
    [Table("FinancialAccount")]
    public class FinancialAccount
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FinancialAccountId { get; set; }

        [Required]
        [MaxLength(200)]
        public required string FinancialAccountName { get; set; }


        #region Transaction Relation

        public virtual ICollection<Transaction> Transactions { get; } = new List<Transaction>();

        #endregion Transaction Relation
    }
}