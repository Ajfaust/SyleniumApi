using FastEndpoints;
using SyleniumApi.DbContexts;
using ILogger = Serilog.ILogger;

namespace SyleniumApi.Features.TransactionCategories;

public record DeleteTransactionCategoryCommand(int Id);

public class DeleteTransactionCategoryEndpoint(SyleniumDbContext context, ILogger logger)
    : Endpoint<DeleteTransactionCategoryCommand>
{
    public override void Configure()
    {
        Delete("/api/transaction-categories/{Id:int}");
        AllowAnonymous();
        DontThrowIfValidationFails();
    }

    public override async Task HandleAsync(DeleteTransactionCategoryCommand cmd, CancellationToken ct)
    {
        var category = await context.TransactionCategories.FindAsync(cmd.Id);
        if (category is null)
        {
            logger.Error("Could not find transaction category with id {Id}", cmd.Id);
            await SendNotFoundAsync(ct);
            return;
        }

        context.Remove(category);
        await context.SaveChangesAsync(ct);

        await SendNoContentAsync(ct);
    }
}