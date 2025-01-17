using FastEndpoints;
using SyleniumApi.DbContexts;
using ILogger = Serilog.ILogger;

namespace SyleniumApi.Features.FinancialAccountCategories;

public record DeleteFaCategoryCommand(int Id);

public class DeleteFaCategoryEndpoint(SyleniumDbContext context, ILogger logger) : Endpoint<DeleteFaCategoryCommand>
{
    public override void Configure()
    {
        Delete("/api/fa-categories/{Id:int}");
        AllowAnonymous();
        DontThrowIfValidationFails();
    }

    public override async Task HandleAsync(DeleteFaCategoryCommand cmd, CancellationToken ct)
    {
        var faCat = await context.FinancialAccountCategories.FindAsync(cmd.Id);
        if (faCat == null)
        {
            logger.Error("Financial account category with id {Id} not found", cmd.Id);
            await SendNotFoundAsync();
            return;
        }

        context.FinancialAccountCategories.Remove(faCat);
        await context.SaveChangesAsync(ct);

        await SendNoContentAsync();
    }
}