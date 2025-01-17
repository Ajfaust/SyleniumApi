using FastEndpoints;
using SyleniumApi.DbContexts;
using ILogger = Serilog.ILogger;

namespace SyleniumApi.Features.Ledgers;

public record DeleteLedgerCommand(int Id);

public class DeleteLedgerEndpoint(SyleniumDbContext context, ILogger logger) : Endpoint<DeleteLedgerCommand>
{
    public override void Configure()
    {
        Delete("/api/ledgers/{Id:int}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(DeleteLedgerCommand req, CancellationToken ct)
    {
        var ledger = await context.Ledgers.FindAsync(req.Id);
        if (ledger is null)
        {
            logger.Error($"Ledger {req.Id} not found.");
            await SendNotFoundAsync();
        }
        else
        {
            context.Ledgers.Remove(ledger);
            await context.SaveChangesAsync();

            await SendNoContentAsync();
        }
    }
}