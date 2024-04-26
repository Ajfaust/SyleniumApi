using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BudgetUpServer.Entity
{
    public class TransactionCategory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransactionCategoryId { get; set; }

        [Required]
        public string TransactionCategoryName { get; set; }

        #region Parent-Child Relation

        public int? ParentTransactionCategoryId { get; set; }

        public virtual TransactionCategory? ParentTransactionCategory { get; set; }

        public virtual ICollection<TransactionCategory> ChildTransactionCategories { get; } = new List<TransactionCategory>();

        #endregion Parent-Child Relation

        #region Spreadsheet Relation

        public int SpreadsheetId { get; set; }
        public virtual Spreadsheet Spreadsheet { get; set; }

        #endregion Spreadsheet Relation

        #region Transaction Relation

        public virtual ICollection<Transaction> Transactions { get; } = new List<Transaction>();

        #endregion Transaction Relation
    }
}