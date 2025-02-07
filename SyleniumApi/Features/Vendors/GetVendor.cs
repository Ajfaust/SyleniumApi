using FastEndpoints;
using SyleniumApi.Data.Entities;
using SyleniumApi.DbContexts;
using ILogger = Serilog.ILogger;

namespace SyleniumApi.Features.Vendors;

public record GetVendorQuery(int Id);

public record GetVendorResponse(int Id, string Name);

public class GetVendorEndpoint(SyleniumDbContext context, ILogger logger) : Endpoint<GetVendorQuery, GetVendorResponse>
{
    public override void Configure()
    {
        Get("vendors/{Id:int}");
        Description(b => b.Produces(404));
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

        await SendAsync(vendor.ToGetResponse(), cancellation: ct);
    }
}

public static class VendorMappers
{
    public static GetVendorResponse ToGetResponse(this Vendor vendor)
    {
        return new GetVendorResponse(vendor.Id, vendor.Name);
    }
}