using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using SyleniumApi.DbContexts;
using SyleniumApi.Features.Vendors;
using ILogger = Serilog.ILogger;

namespace SyleniumApi.Features.Ledgers.Get;

public record GetLedgerVendorsRequest(int Id);

public record GetLedgerVendorsResponse(List<GetVendorResponse> Vendors);

public class GetLedgerVendorsEndpoint(SyleniumDbContext context, ILogger logger)
    : Endpoint<GetLedgerVendorsRequest, GetLedgerVendorsResponse>
{
    public override void Configure()
    {
        Get("ledgers/{Id:int}/vendors");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetLedgerVendorsRequest req, CancellationToken ct)
    {
        var ledgerExists = await context.Ledgers.AnyAsync(l => l.Id == req.Id, ct);
        if (!ledgerExists)
        {
            var message = $"Unable to find ledger with id {req.Id}";
            logger.Error(message);
            AddError(message);
            await SendErrorsAsync(StatusCodes.Status404NotFound, ct);
            return;
        }

        var vendors = context.Vendors
            .Where(v => v.LedgerId == req.Id)
            .Select(v => v.ToGetResponse())
            .ToList();

        await SendAsync(new GetLedgerVendorsResponse(vendors), cancellation: ct);
    }
}