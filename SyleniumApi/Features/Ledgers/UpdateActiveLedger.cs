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
        logger.Information("Updating active ledger to {id}", cmd.Id);
        var activeLedger = await context.Ledgers.SingleOrDefaultAsync(l => l.IsActive, ct);
        if (activeLedger != null)
        {
            logger.Information("Deactivating ledger {id}", activeLedger.Id);
            activeLedger.IsActive = false;
            context.Update(activeLedger);
            await context.SaveChangesAsync(ct);
        }

        var newActiveLedger = await context.Ledgers.FindAsync(cmd.Id, ct);
        if (newActiveLedger == null)
        {
            var message = $"Unable to find ledger with id {cmd.Id}";
            logger.Error(message);
            AddError(message);
            await SendErrorsAsync(StatusCodes.Status404NotFound, ct);
            return;
        }

        logger.Information("Setting active ledger to {id}", cmd.Id);
        newActiveLedger.IsActive = true;

        context.Update(newActiveLedger);

        await context.SaveChangesAsync(ct);

        await SendOkAsync(ct);
    }
}