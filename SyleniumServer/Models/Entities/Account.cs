using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SyleniumServer.Models.Entities
{
    [Table("Account")]
    public class Account
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AccountId { get; set; }

        [Required]
        public string Name { get; set; }

        public double Balance { get; set; }

        #region Transaction Relation

        public virtual ICollection<Transaction> Transactions { get; } = new List<Transaction>();

        #endregion Transaction Relation
    }
}