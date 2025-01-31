using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SyleniumApi.DbContexts;
using SyleniumApi.Features.FinancialAccounts;
using ILogger = Serilog.ILogger;

namespace SyleniumApi.Features.Ledgers.Get;

public record GetActiveLedgerResponse(
    int Id,
    string Name,
    List<GetFinancialAccountResponse> Accounts
);

public class GetActiveLedgerEndpoint(SyleniumDbContext context, ILogger logger)
    : EndpointWithoutRequest<GetActiveLedgerResponse>
{
    public override void Configure()
    {
        Get("ledgers/active");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var ledgers = await context
            .Ledgers
            .Include(l => l.FinancialAccounts)
            .Where(l => l.IsActive)
            .ToListAsync(ct);

        if (ledgers.IsNullOrEmpty())
        {
            const string message = "Unable to find active ledger";
            logger.Error(message);
            AddError(message);
            await SendErrorsAsync(StatusCodes.Status500InternalServerError, ct);
            return;
        }

        if (ledgers.Count > 1)
        {
            var activeLedgerIds = ledgers.Select(l => l.Id).ToList();
            var message = $"More than one active ledger found: {activeLedgerIds}";
            logger.Error(message);
            AddError(message);
            await SendErrorsAsync(StatusCodes.Status500InternalServerError, ct);
            return;
        }

        var activeLedger = ledgers.Single();
        var accountResponses =
            activeLedger.FinancialAccounts
                .Select(a => new GetFinancialAccountResponse(a.Id, a.Name, null, null))
                .ToList();

        var response = new GetActiveLedgerResponse(
            activeLedger.Id,
            activeLedger.Name,
            accountResponses);
        await SendAsync(response, cancellation: ct);
    }
}