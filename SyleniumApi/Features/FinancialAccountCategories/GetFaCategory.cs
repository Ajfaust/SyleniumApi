using FastEndpoints;
using SyleniumApi.Data.Entities;
using SyleniumApi.DbContexts;
using ILogger = Serilog.ILogger;

namespace SyleniumApi.Features.FinancialAccountCategories;

public record GetFaCategoryRequest(int Id);

public record GetFaCategoryResponse(int Id, string Name, FinancialCategoryType Type);

public class GetFaCategoryMapper : Mapper<GetFaCategoryRequest, GetFaCategoryResponse, FinancialAccountCategory>
{
    public override Task<GetFaCategoryResponse> FromEntityAsync(FinancialAccountCategory cat,
        CancellationToken ct = default)
    {
        return Task.FromResult(new GetFaCategoryResponse(
            cat.Id, cat.Name, cat.Type));
    }
}

public class GetFaCategoryEndpoint(SyleniumDbContext context, ILogger logger)
    : Endpoint<GetFaCategoryRequest, GetFaCategoryResponse, GetFaCategoryMapper>
{
    public override void Configure()
    {
        Get("fa-categories/{Id:int}");
        Description(b => b.Produces(404));
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetFaCategoryRequest req, CancellationToken ct)
    {
        var category = await context.FinancialAccountCategories.FindAsync(req.Id, ct);
        if (category is null)
        {
            logger.Error("Financial account category with id {id} could not be found", req.Id);
            await SendNotFoundAsync(ct);
            return;
        }

        await SendMappedAsync(category, ct: ct);
    }
}