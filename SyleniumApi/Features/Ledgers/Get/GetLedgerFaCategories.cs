using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using SyleniumApi.DbContexts;
using SyleniumApi.Features.FinancialAccountCategories;
using ILogger = Serilog.ILogger;

namespace SyleniumApi.Features.Ledgers.Get;

public record GetLedgerFaCategoriesRequest(int Id);

public record GetLedgerFaCategoriesResponse(List<GetFaCategoryResponse> Categories);

public class GetAllFaCategoriesEndpoint(SyleniumDbContext context, ILogger logger)
    : Endpoint<GetLedgerFaCategoriesRequest, GetLedgerFaCategoriesResponse>
{
    public override void Configure()
    {
        Get("ledgers/{Id}/fa-categories");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetLedgerFaCategoriesRequest req, CancellationToken ct)
    {
        var ledgerExists = await context.Ledgers.AnyAsync(l => l.Id == req.Id, ct);

        if (!ledgerExists)
        {
            var message = $"Unable to find Ledger ${req.Id}";
            logger.Error(message);
            AddError(message);
            await SendErrorsAsync(StatusCodes.Status404NotFound, ct);
            return;
        }

        var response = await context
            .FinancialAccountCategories
            .Where(fa => fa.LedgerId == req.Id)
            .Select(c => new GetFaCategoryResponse(c.Id, c.Name, c.Type))
            .ToListAsync(ct);

        await SendAsync(new GetLedgerFaCategoriesResponse(response), cancellation: ct);
    }
}