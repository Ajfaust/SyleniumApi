using FastEndpoints;
using SyleniumApi.DbContexts;
using ILogger = Serilog.ILogger;

namespace SyleniumApi.Features.Vendors;

public record GetVendorQuery(int Id);

public record GetVendorResponse(int Id, string Name);

public class GetVendorEndpoint(SyleniumDbContext context, ILogger logger) : Endpoint<GetVendorQuery, GetVendorResponse>
{
    public override void Configure()
    {
        Get("/api/vendors/{Id:int}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetVendorQuery qry, CancellationToken ct)
    {
        var vendor = await context.Vendors.FindAsync(qry.Id, ct);
        if (vendor is null)
        {
            logger.Error("Vendor with id {Id} not found", qry.Id);
            await SendNotFoundAsync(ct);
            return;
        }

        var response = new GetVendorResponse(vendor.VendorId, vendor.VendorName);
        await SendAsync(response, cancellation: ct);
    }
}