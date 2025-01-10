using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SyleniumApi.Data.Entities
{
    [Table("Vendor")]
    public class Vendor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VendorId { get; set; }

        [Required]
        [MaxLength(200)]
        public required string VendorName { get; set; }

        public virtual ICollection<Transaction> Transactions { get; } = new List<Transaction>();
    }
}