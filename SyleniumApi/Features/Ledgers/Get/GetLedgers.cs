using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using SyleniumApi.DbContexts;
using ILogger = Serilog.ILogger;

namespace SyleniumApi.Features.Ledgers.Get;

public class GetLedgersEndpoint(SyleniumDbContext context, ILogger logger)
    : EndpointWithoutRequest<List<GetLedgerResponse>>
{
    public override void Configure()
    {
        Get("ledgers");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var ledgers = await context.Ledgers.ToListAsync(ct);
        var response = ledgers.Select(l => new GetLedgerResponse(l.Id, l.Name)).ToList();
        await SendAsync(response);
    }
}