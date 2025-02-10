using SyleniumApi.Data.Entities;
using SyleniumApi.Features.FinancialAccounts;
using SyleniumApi.Features.TransactionCategories;
using SyleniumApi.Features.Vendors;

namespace SyleniumApi.Features.Transactions;

public static class Mappers
{
    public static GetTransactionResponse ToGetResponse(this Transaction transaction)
    {
        return new GetTransactionResponse(
            transaction.Id,
            transaction.FinancialAccount!.ToGetResponse(),
            transaction.TransactionCategory!.ToGetResponse(),
            transaction.Vendor!.ToGetResponse(),
            transaction.Date,
            transaction.Description ?? string.Empty,
            transaction.Inflow,
            transaction.Outflow,
            transaction.Cleared
        );
    }

    public static UpdateTransactionResponse ToUpdateResponse(this Transaction transaction)
    {
        return new UpdateTransactionResponse(
            transaction.Id,
            transaction.FinancialAccountId,
            transaction.TransactionCategoryId,
            transaction.VendorId,
            transaction.Date,
            transaction.Description ?? string.Empty,
            transaction.Inflow,
            transaction.Outflow,
            transaction.Cleared
        );
    }

    public static CreateTransactionResponse ToCreateResponse(this Transaction transaction)
    {
        return new CreateTransactionResponse(
            transaction.Id,
            transaction.FinancialAccountId,
            transaction.TransactionCategoryId,
            transaction.VendorId,
            transaction.Date,
            transaction.Description ?? string.Empty,
            transaction.Inflow,
            transaction.Outflow,
            transaction.Cleared
        );
    }
}