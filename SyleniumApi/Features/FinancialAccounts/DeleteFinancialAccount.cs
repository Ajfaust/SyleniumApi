using FastEndpoints;
using SyleniumApi.DbContexts;
using ILogger = Serilog.ILogger;

namespace SyleniumApi.Features.FinancialAccounts;

public record DeleteFinancialAccountCommand(int Id);

public class DeleteFinancialAccountEndpoint(SyleniumDbContext context, ILogger logger)
    : Endpoint<DeleteFinancialAccountCommand>
{
    public override void Configure()
    {
        Delete("financial-accounts/{Id:int}");
        Description(b => b.Produces(404));
        AllowAnonymous();
    }

    public override async Task HandleAsync(DeleteFinancialAccountCommand cmd, CancellationToken ct)
    {
        var fa = await context.FinancialAccounts.FindAsync(cmd.Id);
        if (fa is null)
        {
            logger.Error("Could not find financial account with id {Id}", cmd.Id);
            await SendNotFoundAsync(ct);
            return;
        }

        context.FinancialAccounts.Remove(fa);
        await context.SaveChangesAsync(ct);

        await SendNoContentAsync(ct);
    }
}