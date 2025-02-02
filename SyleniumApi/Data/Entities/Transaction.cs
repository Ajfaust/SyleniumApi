﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SyleniumApi.Data.Entities;

[Table("Transaction")]
public class Transaction
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public DateTime Date { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }

    public decimal Inflow { get; set; }

    public decimal Outflow { get; set; }

    public bool Cleared { get; set; }

    #region Vendor Relation

    public int VendorId { get; set; }

    public virtual Vendor? Vendor { get; set; }

    #endregion Vendor Relation

    #region Transaction Category Relation

    public int TransactionCategoryId { get; set; }

    public virtual TransactionCategory? TransactionCategory { get; set; }

    #endregion Transaction Category Relation

    #region Financial Account Relation

    [Required]
    public required int FinancialAccountId { get; set; }

    public virtual FinancialAccount? FinancialAccount { get; set; }

    #endregion Financial Account Relation
}