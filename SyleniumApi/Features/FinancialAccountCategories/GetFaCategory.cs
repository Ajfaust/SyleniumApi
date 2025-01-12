using Carter;
using FluentResults;
using MediatR;
using SyleniumApi.Data.Entities;
using SyleniumApi.DbContexts;

namespace SyleniumApi.Features.FinancialAccountCategories;

public record GetFaCategoryRequest(int Id) : IRequest<Result<GetFaCategoryResponse>>;

public record GetFaCategoryResponse(int Id, string Name, FinancialCategoryType Type);

public class GetFaCategoryHandler(SyleniumDbContext context)
    : IRequestHandler<GetFaCategoryRequest, Result<GetFaCategoryResponse>>
{
    public async Task<Result<GetFaCategoryResponse>> Handle(GetFaCategoryRequest request,
        CancellationToken cancellationToken)
    {
        var faCategory = await context.FinancialAccountCategories.FindAsync(request.Id);
        if (faCategory is null) return Result.Fail($"Financial Account Category {request.Id} not found");

        var response = new GetFaCategoryResponse(faCategory.FinancialAccountCategoryId,
            faCategory.FinancialAccountCategoryName, faCategory.FinancialCategoryType);

        return Result.Ok(response);
    }
}

public class GetFaCategoryEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/fa-categories/{id:int}", async (int id, ISender sender) =>
        {
            var command = new GetFaCategoryRequest(id);

            var result = await sender.Send(command);

            return result.IsFailed ? Result.Fail(result.Errors) : Result.Ok(result.Value);
        });
    }
}