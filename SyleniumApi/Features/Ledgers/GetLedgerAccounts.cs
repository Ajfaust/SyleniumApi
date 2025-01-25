using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using SyleniumApi.DbContexts;
using SyleniumApi.Features.FinancialAccounts;
using ILogger = Serilog.ILogger;

namespace SyleniumApi.Features.Ledgers;

public record GetLedgerAccountsRequest(int Id);

public record GetLedgerAccountsResponse(List<GetFinancialAccountResponse> Accounts);

public class GetLedgerAccountsEndpoint(SyleniumDbContext context, ILogger logger)
    : Endpoint<GetLedgerAccountsRequest, GetLedgerAccountsResponse>
{
    public override void Configure()
    {
        Get("ledgers/{Id:int}/accounts");
        Description(b => b.Produces(400));
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetLedgerAccountsRequest req, CancellationToken ct)
    {
        var ledgerExists = await context.Ledgers.AnyAsync(l => l.Id == req.Id, ct);
        if (!ledgerExists)
        {
            var message = $"Ledger {req.Id} does not exist";
            logger.Error(message);
            AddError(message);
            await SendErrorsAsync(cancellation: ct);
            return;
        }

        var accounts = context
            .FinancialAccounts
            .Where(fa => fa.LedgerId == req.Id)
            .Select(fa => new GetFinancialAccountResponse(fa.Id, fa.Name))
            .ToList();

        await SendAsync(new GetLedgerAccountsResponse(accounts), cancellation: ct);
    }
}