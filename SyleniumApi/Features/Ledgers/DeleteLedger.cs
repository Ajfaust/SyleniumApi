using FastEndpoints;
using SyleniumApi.DbContexts;
using ILogger = Serilog.ILogger;

namespace SyleniumApi.Features.Ledgers;

public record DeleteLedgerCommand(int Id);

public class DeleteLedgerEndpoint(SyleniumDbContext context, ILogger logger) : Endpoint<DeleteLedgerCommand>
{
    public override void Configure()
    {
        Delete("ledgers/{Id:int}");
        Description(b => b.Produces(404));
        AllowAnonymous();
    }

    public override async Task HandleAsync(DeleteLedgerCommand req, CancellationToken ct)
    {
        var ledger = await context.Ledgers.FindAsync(req.Id, ct);
        if (ledger is null)
        {
            logger.Error($"Ledger {req.Id} not found.");
            await SendNotFoundAsync(ct);
        }
        else
        {
            context.Ledgers.Remove(ledger);
            await context.SaveChangesAsync(ct);

            await SendNoContentAsync(ct);
        }
    }
}