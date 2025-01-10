using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SyleniumApi.Models.Entities
{
    // TODO: Maybe rename to User Account? Idea was to have 1:1 user account to journal anyway
    [Table("Journal")]
    public class Journal
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int JournalId { get; set; }
        
        [Required]
        [MaxLength(200)]
        public required string JournalName { get; set; }
    }
}

