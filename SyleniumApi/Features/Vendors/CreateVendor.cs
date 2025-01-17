using FastEndpoints;
using FluentValidation;
using SyleniumApi.Data.Entities;
using SyleniumApi.DbContexts;
using ILogger = Serilog.ILogger;

namespace SyleniumApi.Features.Vendors;

public record CreateVendorCommand(int LedgerId, string Name);

public record CreateVendorResponse(int Id, string Name);

public class CreateVendorMapper : Mapper<CreateVendorCommand, CreateVendorResponse, Vendor>
{
    public override Vendor ToEntity(CreateVendorCommand cmd)
    {
        return new Vendor
        {
            LedgerId = cmd.LedgerId,
            VendorName = cmd.Name
        };
    }

    public override CreateVendorResponse FromEntity(Vendor v)
    {
        return new CreateVendorResponse(v.VendorId, v.VendorName);
    }
}

public class CreateVendorValidator : Validator<CreateVendorCommand>
{
    public CreateVendorValidator()
    {
        RuleFor(x => x.LedgerId).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
    }
}

public class CreateVendorEndpoint(SyleniumDbContext context, ILogger logger)
    : Endpoint<CreateVendorCommand, CreateVendorResponse, CreateVendorMapper>
{
    public override void Configure()
    {
        Post("/api/vendors");
        AllowAnonymous();
        DontThrowIfValidationFails();
    }

    public override async Task HandleAsync(CreateVendorCommand cmd, CancellationToken ct)
    {
        if (ValidationFailed)
        {
            logger.Error("Validation failed for CreateVendor");
            foreach (var f in ValidationFailures)
                logger.Error("{0}: {1}", f.PropertyName, f.ErrorMessage);

            await SendErrorsAsync(cancellation: ct);
            return;
        }

        var vendor = Map.ToEntity(cmd);
        await context.Vendors.AddAsync(vendor, ct);
        await context.SaveChangesAsync(ct);

        await SendMappedAsync(vendor, StatusCodes.Status201Created);
    }
}