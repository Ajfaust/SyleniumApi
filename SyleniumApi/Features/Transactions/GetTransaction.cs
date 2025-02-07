using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using SyleniumApi.Data.Entities;
using SyleniumApi.DbContexts;
using SyleniumApi.Features.TransactionCategories;
using SyleniumApi.Features.Vendors;
using ILogger = Serilog.ILogger;

namespace SyleniumApi.Features.Transactions;

public record GetTransactionRequest(int Id);

public record GetTransactionResponse(
    int Id,
    int AccountId,
    GetTransactionCategoryResponse Category,
    GetVendorResponse Vendor,
    DateTime Date,
    string Description,
    decimal Inflow,
    decimal Outflow,
    bool Cleared
);

public class GetTransactionEndpoint(SyleniumDbContext context, ILogger logger)
    : Endpoint<GetTransactionRequest, GetTransactionResponse>
{
    public override void Configure()
    {
        Get("transactions/{Id:int}");
        Description(b => b.Produces(404));
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetTransactionRequest req, CancellationToken ct)
    {
        var transaction = await context
            .Transactions
            .Include(t => t.FinancialAccount)
            .Include(t => t.TransactionCategory)
            .SingleOrDefaultAsync(t => t.Id == req.Id, ct);
        if (transaction is null)
        {
            logger.Error("Unable to find transaction with id {Id}", req.Id);
            await SendNotFoundAsync(ct);
            return;
        }

        try
        {
            await SendAsync(transaction.ToGetResponse(), cancellation: ct);
        }
        catch (NullReferenceException ex)
        {
            logger.Error(ex.Message);
            AddError("Something went wrong building response object");
            await SendErrorsAsync(StatusCodes.Status500InternalServerError, ct);
        }
    }
}

public static class TransactionMappers
{
    public static GetTransactionResponse ToGetResponse(this Transaction transaction)
    {
        if (transaction is { Vendor: not null, TransactionCategory: not null })
            return new GetTransactionResponse(
                transaction.Id, transaction.FinancialAccountId, transaction.TransactionCategory.ToGetResponse(),
                transaction.Vendor.ToGetResponse(), transaction.Date, transaction.Description ?? string.Empty,
                transaction.Inflow,
                transaction.Outflow, transaction.Cleared);
        
        var message = transaction.Vendor == null ? "Vendor cannot be null" : "TransactionCategory cannot be null";
        throw new NullReferenceException(message);

    }
}