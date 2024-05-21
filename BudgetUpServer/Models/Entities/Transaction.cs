using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BudgetUpServer.Models.Entities
{
    [Table("Transaction")]
    public class Transaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransactionId { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public string? Notes { get; set; }

        public decimal Inflow { get; set; }

        public decimal Outflow { get; set; }

        public bool Cleared { get; set; }

        #region Vendor Relation

        public int? VendorId { get; set; }

        public virtual Vendor? Vendor { get; set; }

        #endregion Vendor Relation

        #region Transaction Category Relation

        public int? TransactionCategoryId { get; set; }

        public virtual TransactionCategory? TransactionCategory { get; set; }

        #endregion Transaction Category Relation

        #region FinancialAccount Relation

        public int? FinancialAccountId { get; set; }

        public virtual FinancialAccount? FinancialAccount { get; set; }

        #endregion FinancialAccount Relation
    }
}