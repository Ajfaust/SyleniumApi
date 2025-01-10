using Carter;
using FluentResults;
using MediatR;
using SyleniumApi.Data.Entities;
using SyleniumApi.DbContexts;
using SyleniumApi.Features.FinancialAccountCategories.Contracts;

namespace SyleniumApi.Features.FinancialAccountCategories;

public class CreateFaCategory
{
    public class Request : IRequest<Result<FaCategoryResponse>>
    {
        public string CategoryName { get; set; } = string.Empty;
        public FinancialCategoryType CategoryType { get; set; }
    }

    internal sealed class Handler(SyleniumDbContext context) : IRequestHandler<Request, Result<FaCategoryResponse>>
    {
        public async Task<Result<FaCategoryResponse>> Handle(Request request, CancellationToken cancellationToken)
        {
            var category = new FinancialAccountCategory
            {
                FinancialAccountCategoryName = request.CategoryName,
                FinancialCategoryType = request.CategoryType,
            };

            context.FinancialAccountCategories.Add(category);
            await context.SaveChangesAsync(cancellationToken);

            return Result.Ok();
        }
    }
}

public class CreateFinancialAccountCategoryEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/fa_categories", async (CreateFaCategoryRequest req, ISender sender) =>
        {
            var command = req.MapCreateFaCategoryRequest();

            var result = await sender.Send(command);
            
            return result.IsFailed ? Result.Fail(result.Errors) : Result.Ok(result);
        });
    }
}