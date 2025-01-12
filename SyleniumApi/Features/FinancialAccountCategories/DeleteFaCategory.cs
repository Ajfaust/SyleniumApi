using Carter;
using FluentResults;
using MediatR;
using SyleniumApi.DbContexts;

namespace SyleniumApi.Features.FinancialAccountCategories;

public record DeleteFaCategoryRequest(int Id) : IRequest<Result>;

public class DeleteFaCategoryHandler(SyleniumDbContext context) : IRequestHandler<DeleteFaCategoryRequest, Result>
{
    public async Task<Result> Handle(DeleteFaCategoryRequest request, CancellationToken cancellationToken)
    {
        var category = await context.FinancialAccountCategories.FindAsync(request.Id);
        if (category is null) return Result.Fail($"Category {request.Id} not found");

        context.FinancialAccountCategories.Remove(category);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}

public class DeleteFaCategoryEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/fa-categories/{id:int}", async (int id, ISender sender) =>
        {
            var command = new DeleteFaCategoryRequest(id);

            var result = await sender.Send(command);

            return result.IsFailed ? Result.Fail(result.Errors) : Result.Ok();
        });
    }
}