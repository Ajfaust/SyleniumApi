using FastEndpoints;
using FluentValidation;
using SyleniumApi.Data.Entities;
using SyleniumApi.DbContexts;
using SyleniumApi.Features.Shared;
using ILogger = Serilog.ILogger;

namespace SyleniumApi.Features.Vendors;

public record UpdateVendorCommand(int Id, string Name);

public record UpdateVendorResponse(int Id, string Name);

public class UpdateVendorMapper : Mapper<UpdateVendorCommand, UpdateVendorResponse, Vendor>
{
    public override Task<UpdateVendorResponse> FromEntityAsync(Vendor v, CancellationToken ct = default)
    {
        return Task.FromResult(new UpdateVendorResponse(v.VendorId, v.VendorName));
    }
}

public class UpdateVendorValidator : Validator<UpdateVendorCommand>
{
    public UpdateVendorValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
    }
}

public class UpdateVendorEndpoint(SyleniumDbContext context, ILogger logger)
    : Endpoint<UpdateVendorCommand, UpdateVendorResponse, UpdateVendorMapper>
{
    public override void Configure()
    {
        Put("vendors/{Id:int}");
        AllowAnonymous();
    }

    public override void OnValidationFailed()
    {
        logger.LogValidationErrors(nameof(UpdateVendorEndpoint), ValidationFailures);
    }

    public override async Task HandleAsync(UpdateVendorCommand cmd, CancellationToken ct)
    {
        var vendor = await context.Vendors.FindAsync(cmd.Id, ct);
        if (vendor is null)
        {
            logger.Error("Vendor with id {Id} not found", cmd.Id);
            await SendNotFoundAsync(ct);
            return;
        }

        vendor.VendorName = cmd.Name;
        context.Vendors.Update(vendor);
        await context.SaveChangesAsync(ct);

        await SendMappedAsync(vendor, ct: ct);
    }
}