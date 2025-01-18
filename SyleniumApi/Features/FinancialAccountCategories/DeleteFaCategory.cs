using FastEndpoints;
using SyleniumApi.DbContexts;
using ILogger = Serilog.ILogger;

namespace SyleniumApi.Features.FinancialAccountCategories;

public record DeleteFaCategoryCommand(int Id);

public class DeleteFaCategoryEndpoint(SyleniumDbContext context, ILogger logger) : Endpoint<DeleteFaCategoryCommand>
{
    public override void Configure()
    {
        Delete("fa-categories/{Id:int}");
        Description(b => b.Produces(404));
        AllowAnonymous();
    }

    public override async Task HandleAsync(DeleteFaCategoryCommand cmd, CancellationToken ct)
    {
        var faCat = await context.FinancialAccountCategories.FindAsync(cmd.Id);
        if (faCat == null)
        {
            logger.Error("Financial account category with id {Id} not found", cmd.Id);
            await SendNotFoundAsync(ct);
            return;
        }

        context.FinancialAccountCategories.Remove(faCat);
        await context.SaveChangesAsync(ct);

        await SendNoContentAsync(ct);
    }
}