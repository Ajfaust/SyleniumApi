using FastEndpoints;
using SyleniumApi.DbContexts;
using ILogger = Serilog.ILogger;

namespace SyleniumApi.Features.TransactionCategories;

public record GetTransactionCategoryRequest(int Id);

public record GetTransactionCategoryResponse(int Id, int? ParentId, string Name);

public class GetTransactionCategoryEndpoint(SyleniumDbContext context, ILogger logger)
    : Endpoint<GetTransactionCategoryRequest, GetTransactionCategoryResponse>
{
    public override void Configure()
    {
        Get("/api/transaction-categories/{Id:int}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetTransactionCategoryRequest cmd, CancellationToken ct)
    {
        var category = await context.TransactionCategories.FindAsync(cmd.Id);
        if (category is null)
        {
            logger.Error("Could not find transaction category with id {Id}", cmd.Id);
            await SendNotFoundAsync(ct);
            return;
        }

        var response = new GetTransactionCategoryResponse(category.TransactionCategoryId, category.ParentCategoryId,
            category.TransactionCategoryName);
        await SendOkAsync(response, ct);
    }
}