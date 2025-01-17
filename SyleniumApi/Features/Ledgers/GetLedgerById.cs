using FastEndpoints;
using SyleniumApi.DbContexts;
using ILogger = Serilog.ILogger;

namespace SyleniumApi.Features.Ledgers;

public record GetLedgerRequest(int Id);

public record GetLedgerResponse(int Id, string Name);

public class GetLedgerEndpoint(SyleniumDbContext context, ILogger logger)
    : Endpoint<GetLedgerRequest, GetLedgerResponse>
{
    public override void Configure()
    {
        Get("/api/ledgers/{Id:int}");
        AllowAnonymous();
        DontThrowIfValidationFails();
    }

    public override async Task HandleAsync(GetLedgerRequest req, CancellationToken ct)
    {
        var ledger = await context.Ledgers.FindAsync(req.Id);
        if (ledger is null)
        {
            logger.Error("Ledger {id} not found.", req.Id);
            await SendNotFoundAsync();
        }
        else
        {
            logger.Information("Successfully retrieved ledger {id}", ledger.LedgerId);
            await SendOkAsync(new GetLedgerResponse(ledger.LedgerId, ledger.LedgerName));
        }
    }
}