using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using SyleniumApi.DbContexts;
using SyleniumApi.Features.FinancialAccounts;
using ILogger = Serilog.ILogger;

namespace SyleniumApi.Features.Ledgers.Get;

public record GetLedgerAccountsRequest(int Id);

public record GetLedgerAccountsResponse(List<GetFinancialAccountResponse> Accounts);

public class GetLedgerAccountsEndpoint(SyleniumDbContext context, ILogger logger)
    : Endpoint<GetLedgerAccountsRequest, GetLedgerAccountsResponse>
{
    public override void Configure()
    {
        Get("ledgers/{Id:int}/financial-accounts");
        Description(b => b.Produces(400));
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetLedgerAccountsRequest req, CancellationToken ct)
    {
        logger.Information("Retrieving accounts for ledger {id}", req.Id);
        var ledgerExists = await context.Ledgers.AnyAsync(l => l.Id == req.Id, ct);
        if (!ledgerExists)
        {
            var message = $"Ledger {req.Id} does not exist";
            logger.Error(message);
            AddError(message);
            await SendErrorsAsync(cancellation: ct);
            return;
        }

        try
        {
            var accounts = context
                .FinancialAccounts
                .Where(fa => fa.LedgerId == req.Id)
                .Include(fa => fa.FinancialAccountCategory)
                .Select(fa => fa.ToGetResponse())
                .ToList();

            await SendAsync(new GetLedgerAccountsResponse(accounts), cancellation: ct);
        }
        catch (NullReferenceException ex)
        {
            logger.Error(ex.Message);
            AddError("Unexpected error building response object");
            await SendErrorsAsync(StatusCodes.Status500InternalServerError, ct);
        }
    }
}