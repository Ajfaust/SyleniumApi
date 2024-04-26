using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BudgetUpServer.Entity
{
    public class Spreadsheet
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SpreadsheetId { get; set; }

        #region Transaction Relation

        public virtual ICollection<Transaction> Transactions { get; set; }

        #endregion Transaction Relation

        #region Vendor Relation

        public virtual ICollection<Vendor> Vendors { get; set; }

        #endregion Vendor Relation

        #region Category Relation

        public virtual ICollection<TransactionCategory> Categories { get; set; }

        #endregion Category Relation

        #region Account Relation

        public virtual ICollection<Account> Accounts { get; set; }

        #endregion Account Relation
    }
}