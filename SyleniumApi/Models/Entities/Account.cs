using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SyleniumApi.Models.Entities
{
    [Table("Account")]
    public class Account
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AccountId { get; set; }

        [Required]
        [MaxLength(200)]
        public required string AccountName { get; set; }


        #region Transaction Relation

        public virtual ICollection<Transaction> Transactions { get; } = new List<Transaction>();

        #endregion Transaction Relation
    }
}