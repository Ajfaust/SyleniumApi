using Carter;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyleniumApi.DbContexts;

namespace SyleniumApi.Features.Ledgers;

public record GetLedgerRequest(int Id) : IRequest<Result<GetLedgerResponse>>;

public record GetLedgerResponse(int Id, string Name);

public class GetLedgerHandler(SyleniumDbContext context) : IRequestHandler<GetLedgerRequest, Result<GetLedgerResponse>>
{
    public async Task<Result<GetLedgerResponse>> Handle(GetLedgerRequest request, CancellationToken cancellationToken)
    {
        var ledger = await context.Ledgers.FindAsync(request.Id);
        if (ledger is null)
        {
            return Result.Fail($"Ledger {request.Id} not found");
        }

        var response = new GetLedgerResponse(Id: ledger.LedgerId, Name: ledger.LedgerName);

        return Result.Ok(response);
    }
}
public partial class LedgersController
{
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetLedger(int id, ISender sender)
    {
        var request = new GetLedgerRequest(id);
        var result = await sender.Send(request);

        return result.IsFailed ? NotFound(result.Errors) : Ok(result.Value);
    }
}