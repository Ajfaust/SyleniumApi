using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyleniumApi.DbContexts;
using SyleniumApi.Shared;

namespace SyleniumApi.Features.Ledgers;

public record GetLedgerRequest(int Id) : IRequest<Result<GetLedgerResponse>>;

public record GetLedgerResponse(int Id, string Name);

public class GetLedgerHandler(SyleniumDbContext context) : IRequestHandler<GetLedgerRequest, Result<GetLedgerResponse>>
{
    public async Task<Result<GetLedgerResponse>> Handle(GetLedgerRequest request, CancellationToken cancellationToken)
    {
        var ledger = await context.Ledgers.FindAsync(request.Id);
        if (ledger is null)
            return Result.Fail($"Ledger {request.Id} not found");

        var response = new GetLedgerResponse(ledger.LedgerId, ledger.LedgerName);

        return Result.Ok(response);
    }
}

public partial class LedgersController
{
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetLedger(int id, ISender sender)
    {
        try
        {
            var request = new GetLedgerRequest(id);
            var result = await sender.Send(request);

            if (result.HasError<EntityNotFoundError>())
            {
                logger.LogNotFoundError(result);
                return NotFound(result.Errors);
            }

            logger.Information($"Got Ledger {id} successfully");
            return Ok(result.Value);
        }
        catch (Exception ex)
        {
            var message = $"Unexpected error retrieving Ledger {id}";
            logger.Error(ex, message);
            return StatusCode(StatusCodes.Status500InternalServerError, message);
        }
    }
}