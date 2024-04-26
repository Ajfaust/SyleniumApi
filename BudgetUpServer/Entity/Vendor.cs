using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetUpServer.Entity
{
    public class Vendor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VendorId { get; set; }

        [Required]
        public string VendorName { get; set; }

        public int ProfileId { get; set; }
        public virtual Profile Profile { get; set; }

        public virtual ICollection<Transaction> Transactions { get; } = new List<Transaction>();
    }
}
