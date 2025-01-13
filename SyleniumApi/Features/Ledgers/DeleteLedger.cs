using Carter;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyleniumApi.DbContexts;

namespace SyleniumApi.Features.Ledgers;

public record DeleteLedgerCommand(int Id) : IRequest<Result>;

public class DeleteLedgerHandler(SyleniumDbContext context) : IRequestHandler<DeleteLedgerCommand, Result>
{
    public async Task<Result> Handle(DeleteLedgerCommand request, CancellationToken cancellationToken)
    {
        var ledger = await context.Ledgers.FindAsync(request.Id);
        if (ledger is null)
        {
            return Result.Fail($"Ledger {request.Id} not found");
        }

        context.Ledgers.Remove(ledger);

        await context.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}

public partial class LedgersController
{
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteLedger(int id, ISender sender)
    {
        var command = new DeleteLedgerCommand(id);
        var result = await sender.Send(command);

        return result.IsFailed ? BadRequest(result.Errors) : NoContent();
    }
}