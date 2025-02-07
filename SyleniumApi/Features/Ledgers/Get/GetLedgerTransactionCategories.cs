using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using SyleniumApi.DbContexts;
using SyleniumApi.Features.TransactionCategories;
using ILogger = Serilog.ILogger;

namespace SyleniumApi.Features.Ledgers.Get;

public record GetLedgerTransactionCategoriesRequest(int Id);

public record GetLedgerTransactionCategoriesResponse(List<GetTransactionCategoryResponse> Categories);

public class GetLedgerTransactionCategoriesEndpoint(SyleniumDbContext context, ILogger logger)
    : Endpoint<GetLedgerTransactionCategoriesRequest, GetLedgerTransactionCategoriesResponse>
{
    public override void Configure()
    {
        Get("ledgers/{Id:int}/categories");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetLedgerTransactionCategoriesRequest req, CancellationToken ct)
    {
        var ledgerExists = await context.Ledgers.AnyAsync(l => l.Id == req.Id, ct);
        if (!ledgerExists)
        {
            var message = $"Unable to find ledger {req.Id}";
            logger.Error(message);
            AddError(message);
            await SendErrorsAsync(StatusCodes.Status404NotFound, ct);
            return;
        }

        var categories = context.TransactionCategories
            .Where(c => c.LedgerId == req.Id && c.ParentCategoryId == null)
            .Select(c => c.ToGetResponse())
            .ToList();

        await SendAsync(new GetLedgerTransactionCategoriesResponse(categories), cancellation: ct);
    }
}