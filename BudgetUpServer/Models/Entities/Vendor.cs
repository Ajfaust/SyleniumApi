using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BudgetUpServer.Models.Entities
{
    [Table("Vendor")]
    public class Vendor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VendorId { get; set; }

        [Required]
        public string Name { get; set; }

        public int LedgerId { get; set; }
        public virtual Ledger Ledger { get; set; }

        public virtual ICollection<Transaction> Transactions { get; } = new List<Transaction>();
    }
}