using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BudgetUpServer.Entity
{
    [Table("TransactionCategory")]
    public class TransactionCategory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransactionCategoryId { get; set; }

        [Required]
        public string Name { get; set; }

        #region Parent-Child Relation

        public int? ParentId { get; set; }

        public virtual TransactionCategory? ParentCategory { get; set; }

        public virtual ICollection<TransactionCategory> SubCategories { get; } = new List<TransactionCategory>();

        #endregion Parent-Child Relation

        #region Portfolio Relation

        public int PortfolioId { get; set; }
        public virtual Portfolio Portfolio { get; set; }

        #endregion Portfolio Relation

        #region Transaction Relation

        public virtual ICollection<Transaction> Transactions { get; } = new List<Transaction>();

        #endregion Transaction Relation
    }
}