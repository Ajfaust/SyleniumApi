using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyleniumApi.DbContexts;
using SyleniumApi.Shared;

namespace SyleniumApi.Features.FinancialAccounts;

public record DeleteFinancialAccountCommand(int Id) : IRequest<Result>;

public class DeleteFinancialAccountHandler(SyleniumDbContext context)
    : IRequestHandler<DeleteFinancialAccountCommand, Result>
{
    public async Task<Result> Handle(DeleteFinancialAccountCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.FinancialAccounts.FindAsync(request.Id);
        if (entity is null)
            return new EntityNotFoundError("Financial account not found");

        context.FinancialAccounts.Remove(entity);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}

public partial class FinancialAccountsController
{
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteFinancialAccount(int id, ISender sender)
    {
        try
        {
            var result = await sender.Send(new DeleteFinancialAccountCommand(id));

            if (result.HasError<EntityNotFoundError>())
            {
                logger.LogNotFoundError(result);
                return NotFound(result.Errors);
            }

            logger.Information($"Successfully deleted financial account with Id: {id}");
            return NoContent();
        }
        catch (Exception ex)
        {
            var message = $"Unexpected error deleting financial account with Id: {id}";
            logger.Error(ex, message);
            return StatusCode(StatusCodes.Status500InternalServerError, message);
        }
    }
}