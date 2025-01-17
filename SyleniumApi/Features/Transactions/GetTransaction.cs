using FastEndpoints;
using SyleniumApi.Data.Entities;
using SyleniumApi.DbContexts;
using ILogger = Serilog.ILogger;

namespace SyleniumApi.Features.Transactions;

public record GetTransactionRequest(int Id);

public record GetTransactionResponse(TransactionDto Dto);

public class GetTransactionMapper : Mapper<GetTransactionRequest, GetTransactionResponse, Transaction>
{
    public override Task<GetTransactionResponse> FromEntityAsync(Transaction e, CancellationToken ct = default)
    {
        var dto = new TransactionDto
        {
            Id = e.TransactionId,
            AccountId = e.FinancialAccountId,
            CategoryId = e.TransactionCategoryId,
            VendorId = e.VendorId,
            Description = e.Description ?? string.Empty,
            Date = e.Date,
            Inflow = e.Inflow,
            Outflow = e.Outflow,
            Cleared = e.Cleared
        };
        return Task.FromResult(new GetTransactionResponse(dto));
    }
}

public class GetTransactionEndpoint(SyleniumDbContext context, ILogger logger)
    : Endpoint<GetTransactionRequest, GetTransactionResponse, GetTransactionMapper>
{
    public override void Configure()
    {
        Get("/api/transactions/{Id:int}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetTransactionRequest req, CancellationToken ct)
    {
        var transaction = await context.Transactions.FindAsync(req.Id, ct);
        if (transaction is null)
        {
            logger.Error("Unable to find transaction with id {Id}", req.Id);
            await SendNotFoundAsync(ct);
            return;
        }

        await SendMappedAsync(transaction, ct: ct);
    }
}