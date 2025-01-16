using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyleniumApi.DbContexts;
using SyleniumApi.Shared;

namespace SyleniumApi.Features.Ledgers;

public record DeleteLedgerCommand(int Id) : IRequest<Result>;

public class DeleteLedgerHandler(SyleniumDbContext context) : IRequestHandler<DeleteLedgerCommand, Result>
{
    public async Task<Result> Handle(DeleteLedgerCommand request, CancellationToken cancellationToken)
    {
        var ledger = await context.Ledgers.FindAsync(request.Id);
        if (ledger is null)
            return new EntityNotFoundError($"Ledger {request.Id} not found");

        context.Ledgers.Remove(ledger);

        await context.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}

public partial class LedgersController
{
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteLedger(int id, ISender sender)
    {
        try
        {
            var command = new DeleteLedgerCommand(id);
            var result = await sender.Send(command);

            if (result.HasError<EntityNotFoundError>())
            {
                logger.LogNotFoundError(result);
                return NotFound(result.Errors);
            }

            logger.Information($"Ledger {id} deleted successfully");
            return NoContent();
        }
        catch (Exception ex)
        {
            var message = $"Unexpected error deleting Ledger {id}";
            logger.Error(message, ex);
            return StatusCode(StatusCodes.Status500InternalServerError, message);
        }
    }
}