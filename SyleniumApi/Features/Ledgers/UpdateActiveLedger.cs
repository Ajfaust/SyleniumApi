using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using SyleniumApi.DbContexts;
using ILogger = Serilog.ILogger;

namespace SyleniumApi.Features.Ledgers;

public record UpdateActiveLedgerCommand(int Id);

public class UpdateActiveLedgerEndpoint(SyleniumDbContext context, ILogger logger) : Endpoint<UpdateActiveLedgerCommand>
{
    public override void Configure()
    {
        Put("ledgers/active");
        AllowAnonymous();
    }

    public override async Task HandleAsync(UpdateActiveLedgerCommand cmd, CancellationToken ct)
    {
        var ledger = await context.Ledgers.FindAsync(cmd.Id, ct);
        if (ledger is null)
        {
            var message = $"Unable to find ledger {cmd.Id}";
            logger.Error(message);
            AddError(message);
            await SendErrorsAsync(StatusCodes.Status404NotFound, ct);
            return;
        }

        var activeLedger = await context.Ledgers.SingleOrDefaultAsync(l => l.IsActive, ct);
        if (activeLedger is not null)
        {
            activeLedger.IsActive = false;
            logger.Information($"Deactivated ledger {activeLedger.Id}");
            context.Ledgers.Update(activeLedger);
            await context.SaveChangesAsync(ct);
        }

        ledger.IsActive = true;
        logger.Information($"Setting active ledger to {ledger.Id}");
        context.Ledgers.Update(ledger);
        await context.SaveChangesAsync(ct);

        await SendOkAsync(ct);
    }
}