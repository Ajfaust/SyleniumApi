using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using SyleniumApi.Data.Entities;
using SyleniumApi.DbContexts;
using ILogger = Serilog.ILogger;

namespace SyleniumApi.Features.Transactions;

public record GetTransactionRequest(int Id);

public record GetTransactionResponse(
    int Id,
    string AccountName,
    string CategoryName,
    string VendorName,
    DateTime Date,
    string Description,
    decimal Inflow,
    decimal Outflow,
    bool Cleared
);

public class GetTransactionMapper : Mapper<GetTransactionRequest, GetTransactionResponse, Transaction>
{
    public override Task<GetTransactionResponse> FromEntityAsync(Transaction e, CancellationToken ct = default)
    {
        return Task.FromResult(new GetTransactionResponse(
            e.Id,
            e.FinancialAccount?.Name ?? string.Empty,
            e.TransactionCategory?.Name ?? string.Empty,
            e.Vendor?.Name ?? string.Empty,
            e.Date,
            e.Description ?? string.Empty,
            e.Inflow,
            e.Outflow,
            e.Cleared
        ));
    }
}

public class GetTransactionEndpoint(SyleniumDbContext context, ILogger logger)
    : Endpoint<GetTransactionRequest, GetTransactionResponse, GetTransactionMapper>
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

        await SendMappedAsync(transaction, ct: ct);
    }
}