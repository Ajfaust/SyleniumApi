using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyleniumApi.DbContexts;
using SyleniumApi.Shared;

namespace SyleniumApi.Features.FinancialAccountCategories;

public record DeleteFaCategoryRequest(int Id) : IRequest<Result>;

public class DeleteFaCategoryHandler(SyleniumDbContext context) : IRequestHandler<DeleteFaCategoryRequest, Result>
{
    public async Task<Result> Handle(DeleteFaCategoryRequest request, CancellationToken cancellationToken)
    {
        var category = await context.FinancialAccountCategories.FindAsync(request.Id);
        if (category is null)
            return new EntityNotFoundError($"Category {request.Id} not found");

        context.FinancialAccountCategories.Remove(category);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}

public partial class FaCategoriesController
{
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteFaCategory(int id, ISender sender)
    {
        var request = new DeleteFaCategoryRequest(id);
        var result = await sender.Send(request);

        return result.HasError<EntityNotFoundError>() ? NotFound(result.Errors) : NoContent();
    }
}