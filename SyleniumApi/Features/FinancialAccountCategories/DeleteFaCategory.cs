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
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteFaCategory(int id, ISender sender)
    {
        try
        {
            var request = new DeleteFaCategoryRequest(id);
            var result = await sender.Send(request);

            if (result.HasError<EntityNotFoundError>())
                logger.LogNotFoundError(result);

            logger.Information($"Successfully deleted financial account category with Id: {id}");
            return NoContent();
        }
        catch (Exception ex)
        {
            var message = $"Unexpected error deleting financial account category with Id: {id}";
            logger.Error(ex, message);
            return StatusCode(StatusCodes.Status500InternalServerError, message);
        }
    }
}