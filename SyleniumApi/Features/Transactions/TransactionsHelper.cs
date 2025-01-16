using SyleniumApi.Data.Entities;

namespace SyleniumApi.Features.Transactions;

public static class TransactionsHelper
{
    public static TransactionDto ToDto(this Transaction t)
    {
        return new TransactionDto
        {
            Id = t.TransactionId,
            AccountId = t.FinancialAccountId,
            CategoryId = t.TransactionCategoryId,
            VendorId = t.VendorId,
            Date = t.Date,
            Description = t.Description ?? string.Empty,
            Inflow = t.Inflow,
            Outflow = t.Outflow,
            Cleared = t.Cleared
        };
    }

    public static Transaction UpdateTransaction(this TransactionDto dto, Transaction transaction)
    {
        transaction.FinancialAccountId = dto.AccountId;
        transaction.TransactionCategoryId = dto.CategoryId;
        transaction.VendorId = dto.VendorId;
        transaction.Date = dto.Date;
        transaction.Description = dto.Description;
        transaction.Inflow = dto.Inflow;
        transaction.Outflow = dto.Outflow;
        transaction.Cleared = dto.Cleared;

        return transaction;
    }
}