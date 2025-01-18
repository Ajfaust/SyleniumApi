using FastEndpoints;
using FluentValidation;
using SyleniumApi.Data.Entities;
using SyleniumApi.DbContexts;
using SyleniumApi.Features.Shared;
using ILogger = Serilog.ILogger;

namespace SyleniumApi.Features.Vendors;

public record CreateVendorCommand(int LedgerId, string Name);

public record CreateVendorResponse(int Id, string Name);

public class CreateVendorMapper : Mapper<CreateVendorCommand, CreateVendorResponse, Vendor>
{
    public override Task<Vendor> ToEntityAsync(CreateVendorCommand cmd, CancellationToken ct = default)
    {
        return Task.FromResult(new Vendor
        {
            LedgerId = cmd.LedgerId,
            VendorName = cmd.Name
        });
    }

    public override Task<CreateVendorResponse> FromEntityAsync(Vendor v, CancellationToken ct = default)
    {
        return Task.FromResult(new CreateVendorResponse(v.VendorId, v.VendorName));
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
    }

    public override void OnValidationFailed()
    {
        logger.LogValidationErrors(nameof(CreateVendorEndpoint), ValidationFailures);
    }

    public override async Task HandleAsync(CreateVendorCommand cmd, CancellationToken ct)
    {
        var vendor = await Map.ToEntityAsync(cmd, ct);
        await context.Vendors.AddAsync(vendor, ct);
        await context.SaveChangesAsync(ct);

        await SendMappedAsync(vendor, StatusCodes.Status201Created, ct);
    }
}