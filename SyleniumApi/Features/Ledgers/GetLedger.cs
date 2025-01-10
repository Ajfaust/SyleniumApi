using Carter;
using FluentResults;
using FluentValidation;
using MediatR;
using Microsoft.Build.Experimental.ProjectCache;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using SyleniumApi.Contracts;
using SyleniumApi.DbContexts;

namespace SyleniumApi.Features.Ledgers;

public static class GetLedger
{
    public class Query : IRequest<Result<LedgerResponse>>
    {
        public int LedgerId { get; set; }
    }

    internal sealed class Handler(SyleniumDbContext context)
        : IRequestHandler<Query, Result<LedgerResponse>>
    {
        public async Task<Result<LedgerResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var ledgerResponse = await context
                .Ledgers
                .Select(ledger => new LedgerResponse
                {
                    LedgerId = ledger.LedgerId,
                    LedgerName = ledger.LedgerName,
                })
                .FirstOrDefaultAsync(cancellationToken);
            
            if (ledgerResponse == null)
            {
                return Result.Fail<LedgerResponse>("Ledger not found");
            }

            return ledgerResponse;
        }
    }
}

public class GetLedgerEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/ledgers/{id:int}", async (int id, ISender sender) =>
        {
            var query = new GetLedger.Query() { LedgerId = id };
            var result = await sender.Send(query);

            return result.IsFailed ? Result.Fail(result.Errors) : Result.Ok(result);
        });
    }
}