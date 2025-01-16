using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyleniumApi.DbContexts;
using SyleniumApi.Shared;

namespace SyleniumApi.Features.TransactionCategories;

public record DeleteTransactionCategoryCommand(int Id) : IRequest<Result>;

public class DeleteTransactionCategoryHandler(SyleniumDbContext context)
    : IRequestHandler<DeleteTransactionCategoryCommand, Result>
{
    public async Task<Result> Handle(DeleteTransactionCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var transactionCategory = await context.TransactionCategories.FindAsync(request.Id);
        if (transactionCategory is null)
            return new EntityNotFoundError("Transaction category not found");

        context.Remove(transactionCategory);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}

public partial class TransactionCategoriesController
{
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, ISender sender)
    {
        var command = new DeleteTransactionCategoryCommand(id);
        var result = await sender.Send(command);

        return result.HasError<EntityNotFoundError>() ? NotFound(result.Errors) : NoContent();
    }
}