using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyleniumApi.DbContexts;
using SyleniumApi.Shared;

namespace SyleniumApi.Features.Transactions;

public record DeleteTransactionCommand(int Id) : IRequest<Result>;

public class DeleteTransactionCommandHandler(SyleniumDbContext context)
    : IRequestHandler<DeleteTransactionCommand, Result>
{
    public async Task<Result> Handle(DeleteTransactionCommand command,
        CancellationToken cancellationToken)
    {
        var transaction = await context.Transactions.FindAsync(command.Id);
        if (transaction == null)
            return new EntityNotFoundError("Transaction not found");

        context.Transactions.Remove(transaction);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}

public partial class TransactionsController
{
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteTransaction(int id, ISender sender)
    {
        try
        {
            var result = await sender.Send(new DeleteTransactionCommand(id));

            if (result.HasError<EntityNotFoundError>())
            {
                logger.LogNotFoundError(result);
                return NotFound(result.Errors);
            }

            logger.Information($"Successfully deleted transaction with Id: {id}");
            return NoContent();
        }
        catch (Exception ex)
        {
            var message = $"Unexpected error deleting transaction with Id: {id}";
            logger.Error(ex, message);
            return StatusCode(StatusCodes.Status500InternalServerError, message);
        }
    }
}