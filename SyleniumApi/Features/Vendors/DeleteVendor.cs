using FastEndpoints;
using SyleniumApi.DbContexts;
using ILogger = Serilog.ILogger;

namespace SyleniumApi.Features.Vendors;

public record DeleteVendorCommand(int Id);

public class DeleteVendorEndpoint(SyleniumDbContext context, ILogger logger) : Endpoint<DeleteVendorCommand>
{
    public override void Configure()
    {
        Delete("vendors/{Id:int}");
        Description(b => b.Produces(404));
        AllowAnonymous();
    }

    public override async Task HandleAsync(DeleteVendorCommand cmd, CancellationToken ct)
    {
        var vendor = await context.Vendors.FindAsync(cmd.Id, ct);
        if (vendor is null)
        {
            logger.Error("Vendor with id {Id} not found", cmd.Id);
            await SendNotFoundAsync(ct);
            return;
        }

        context.Vendors.Remove(vendor);
        await context.SaveChangesAsync(ct);

        await SendNoContentAsync(ct);
    }
}