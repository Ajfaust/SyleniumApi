using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BudgetUpServer.Entity
{
    [Table("Portfolio")]
    public class Portfolio
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PortfolioId { get; set; }

        [Required]
        public string Name { get; set; }

        #region Transaction Relation

        public virtual ICollection<Transaction> Transactions { get; } = new List<Transaction>();

        #endregion Transaction Relation

        #region Vendor Relation

        public virtual ICollection<Vendor> Vendors { get; } = new List<Vendor>();

        #endregion Vendor Relation

        #region Category Relation

        public virtual ICollection<TransactionCategory> TransactionCategories { get; } = new List<TransactionCategory>();

        #endregion Category Relation

        #region FinancialAccount Relation

        public virtual ICollection<FinancialAccount> FinancialAccounts { get; } = new List<FinancialAccount>();

        #endregion FinancialAccount Relation
    }
}