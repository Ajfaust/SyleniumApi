using FastEndpoints;
using SyleniumApi.Data.Entities;
using SyleniumApi.DbContexts;
using ILogger = Serilog.ILogger;

namespace SyleniumApi.Features.FinancialAccountCategories;

public record GetFaCategoryRequest(int Id);

public record GetFaCategoryResponse(int Id, string Name, FinancialCategoryType Type);

public class GetFaCategoryEndpoint(SyleniumDbContext context, ILogger logger)
    : Endpoint<GetFaCategoryRequest, GetFaCategoryResponse>
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

        await SendAsync(category.ToGetResponse(), cancellation: ct);
    }
}

public static class FaCategoryMappers
{
    public static GetFaCategoryResponse ToGetResponse(this FinancialAccountCategory fa)
    {
        return new GetFaCategoryResponse(fa.Id, fa.Name, fa.Type);
    }
}