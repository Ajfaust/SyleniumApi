using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using SyleniumApi.DbContexts;
using ILogger = Serilog.ILogger;

namespace SyleniumApi.Features.TransactionCategories;

public record GetTransactionCategoryRequest(int Id);

public record GetTransactionCategoryResponse(
    int Id,
    int? ParentId,
    string Name,
    List<GetTransactionCategoryResponse> Subcategories);

public class GetTransactionCategoryEndpoint(SyleniumDbContext context, ILogger logger)
    : Endpoint<GetTransactionCategoryRequest, GetTransactionCategoryResponse>
{
    public override void Configure()
    {
        Get("transaction-categories/{Id:int}");
        Description(b => b.Produces(404));
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetTransactionCategoryRequest cmd, CancellationToken ct)
    {
        var category = await context.TransactionCategories
            .Include(c => c.SubCategories)
            .SingleOrDefaultAsync(c => c.Id == cmd.Id, ct);
        if (category is null)
        {
            logger.Error("Could not find transaction category with id {Id}", cmd.Id);
            await SendNotFoundAsync(ct);
            return;
        }

        var response = new GetTransactionCategoryResponse(category.Id, category.ParentCategoryId,
            category.Name,
            category.SubCategories
                .Select(sc => new GetTransactionCategoryResponse(sc.Id, sc.ParentCategoryId, sc.Name, [])).ToList());
        await SendOkAsync(response, ct);
    }
}