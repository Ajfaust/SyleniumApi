using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using SyleniumApi.DbContexts;
using ILogger = Serilog.ILogger;

namespace SyleniumApi.Features.Ledgers.Get;

public record GetActiveLedgerIdResponse(int Id);

public class GetActiveLedgerEndpoint(SyleniumDbContext context, ILogger logger)
    : EndpointWithoutRequest<GetActiveLedgerIdResponse>
{
    public override void Configure()
    {
        Get("ledgers/active");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        logger.Information("Retrieving active ledger id");
        var activeLedgerId = await context.Ledgers.SingleOrDefaultAsync(l => l.IsActive, ct);
        GetActiveLedgerIdResponse response;
        if (activeLedgerId is null)
        {
            logger.Information("No active ledger id");
            response = new GetActiveLedgerIdResponse(-1);
        }
        else
        {
            logger.Information("Got ledger id {id}", activeLedgerId.Id);
            response = new GetActiveLedgerIdResponse(activeLedgerId.Id);
        }

        await SendAsync(response, cancellation: ct);
    }
}