using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using SyleniumApi.Data.Entities;
using SyleniumApi.DbContexts;
using SyleniumApi.Features.FinancialAccounts;
using SyleniumApi.Features.TransactionCategories;
using SyleniumApi.Features.Vendors;
using ILogger = Serilog.ILogger;

namespace SyleniumApi.Features.Transactions;

public record GetTransactionRequest(int Id);

public record GetTransactionResponse(
    int Id,
    GetFinancialAccountResponse Account,
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

        await SendAsync(transaction.ToGetResponse(), cancellation: ct);
    }
}