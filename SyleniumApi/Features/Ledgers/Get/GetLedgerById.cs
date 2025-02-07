using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using SyleniumApi.DbContexts;
using ILogger = Serilog.ILogger;

namespace SyleniumApi.Features.Ledgers.Get;

public record GetLedgerRequest(int Id);

public record GetLedgerResponse(int Id, string Name, bool IsActive);

public class GetLedgerEndpoint(SyleniumDbContext context, ILogger logger)
    : Endpoint<GetLedgerRequest, GetLedgerResponse>
{
    public override void Configure()
    {
        Get("ledgers/{Id:int}");
        Description(b => b.Produces(404));
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetLedgerRequest req, CancellationToken ct)
    {
        var ledger = await context.Ledgers
            .Include(l => l.FinancialAccounts)
            .SingleOrDefaultAsync(l => l.Id == req.Id, ct);

        if (ledger is null)
        {
            logger.Error("Ledger {id} not found.", req.Id);
            await SendNotFoundAsync(ct);
        }
        else
        {
            logger.Information("Successfully retrieved ledger {id}", ledger.Id);
            await SendOkAsync(new GetLedgerResponse(ledger.Id, ledger.Name, ledger.IsActive), ct);
        }
    }
}