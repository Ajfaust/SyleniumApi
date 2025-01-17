using FastEndpoints;
using SyleniumApi.Data.Entities;
using SyleniumApi.DbContexts;
using ILogger = Serilog.ILogger;

namespace SyleniumApi.Features.FinancialAccountCategories;

public record GetFaCategoryRequest(int Id);

public record GetFaCategoryResponse(int Id, string Name, FinancialCategoryType Type);

public class GetFaCategoryMapper : Mapper<GetFaCategoryRequest, GetFaCategoryResponse, FinancialAccountCategory>
{
    public override GetFaCategoryResponse FromEntity(FinancialAccountCategory cat)
    {
        return new GetFaCategoryResponse(
            cat.FinancialAccountCategoryId, cat.FinancialAccountCategoryName, cat.FinancialCategoryType);
    }
}

public class GetFaCategoryEndpoint(SyleniumDbContext context, ILogger logger)
    : Endpoint<GetFaCategoryRequest, GetFaCategoryResponse, GetFaCategoryMapper>
{
    public override void Configure()
    {
        AllowAnonymous();
        DontThrowIfValidationFails();
    }

    public override async Task HandleAsync(GetFaCategoryRequest req, CancellationToken ct)
    {
        var category = await context.FinancialAccountCategories.FindAsync(req.Id);
        if (category is null)
        {
            logger.Error("Financial account category with id {id} could not be found", req.Id);
            await SendNotFoundAsync();
        }

        await SendMappedAsync(category);
    }
}