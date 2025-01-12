using Carter;
using FluentResults;
using MediatR;
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

public class GetLedgerEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/ledgers/{id:int}", async (int id, ISender sender) =>
        {
            var command = new GetLedgerRequest(Id: id);

            var result = await sender.Send(command);

            return result.IsFailed ? Result.Fail(result.Errors) : Result.Ok(result.Value);
        });
    }
}