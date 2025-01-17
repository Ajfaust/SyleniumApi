using FastEndpoints;
using FluentValidation;
using SyleniumApi.Data.Entities;
using SyleniumApi.DbContexts;
using ILogger = Serilog.ILogger;

namespace SyleniumApi.Features.Vendors;

public record UpdateVendorCommand(int Id, string Name);

public record UpdateVendorResponse(int Id, string Name);

public class UpdateVendorMapper : Mapper<CreateVendorCommand, CreateVendorResponse, Vendor>
{
    public override CreateVendorResponse FromEntity(Vendor v)
    {
        return new CreateVendorResponse(v.VendorId, v.VendorName);
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
        Put("/api/vendors/{Id:int}");
        AllowAnonymous();
        DontThrowIfValidationFails();
    }

    public override async Task HandleAsync(UpdateVendorCommand cmd, CancellationToken ct)
    {
        if (ValidationFailed)
        {
            logger.Error("Validation failed for CreateVendor");
            foreach (var f in ValidationFailures)
                logger.Error("{0}: {1}", f.PropertyName, f.ErrorMessage);

            await SendErrorsAsync(cancellation: ct);
            return;
        }

        var vendor = await context.Vendors.FindAsync(cmd.Id, ct);
        if (vendor is null)
        {
            logger.Error("Vendor with id {Id} not found", cmd.Id);
            await SendNotFoundAsync(ct);
            return;
        }

        await SendMappedAsync(vendor);
    }
}