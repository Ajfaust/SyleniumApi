using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BudgetUpServer.Entity
{
    [Table("Vendor")]
    public class Vendor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VendorId { get; set; }

        [Required]
        public string Name { get; set; }

        public int PortfolioId { get; set; }
        public virtual Portfolio Portfolio { get; set; }

        public virtual ICollection<Transaction> Transactions { get; } = new List<Transaction>();
    }
}