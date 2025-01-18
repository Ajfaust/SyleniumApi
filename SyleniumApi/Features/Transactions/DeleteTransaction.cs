using FastEndpoints;
using SyleniumApi.DbContexts;
using ILogger = Serilog.ILogger;

namespace SyleniumApi.Features.Transactions;

public record DeleteTransactionCommand(int Id);

public class DeleteTransactionEndpoint(SyleniumDbContext context, ILogger logger) : Endpoint<DeleteTransactionCommand>
{
    public override void Configure()
    {
        Delete("transactions/{Id:int}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(DeleteTransactionCommand cmd, CancellationToken ct)
    {
        var transaction = await context.Transactions.FindAsync(cmd.Id, ct);

        if (transaction is null)
        {
            logger.Error("Transaction with id {Id} not found", cmd.Id);
            await SendNotFoundAsync(ct);
            return;
        }

        context.Transactions.Remove(transaction);
        await context.SaveChangesAsync(ct);

        await SendNoContentAsync(ct);
    }
}