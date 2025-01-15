using FluentResults;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyleniumApi.Data.Entities;
using SyleniumApi.DbContexts;
using SyleniumApi.Shared;

namespace SyleniumApi.Features.Vendors;

public record CreateVendorCommand(int LedgerId, string Name) : IRequest<Result<CreateVendorResponse>>;

public record CreateVendorResponse(int Id, string Name);

public class CreateVendorValidator : AbstractValidator<CreateVendorCommand>
{
    public CreateVendorValidator()
    {
        RuleFor(x => x.LedgerId).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
    }
}

public class CreateVendorHandler(SyleniumDbContext context, IValidator<CreateVendorCommand> validator)
    : IRequestHandler<CreateVendorCommand, Result<CreateVendorResponse>>
{
    public async Task<Result<CreateVendorResponse>> Handle(CreateVendorCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            return new ValidationError("One or more properties are invalid");

        var entity = new Vendor
        {
            LedgerId = command.LedgerId,
            VendorName = command.Name
        };

        context.Vendors.Add(entity);

        await context.SaveChangesAsync(cancellationToken);

        var response = new CreateVendorResponse(entity.VendorId, entity.VendorName);
        return Result.Ok(response);
    }
}

public partial class VendorsController
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<CreateVendorResponse>> CreateVendor([FromBody] CreateVendorCommand command,
        ISender sender)
    {
        var result = await sender.Send(command);

        return result.HasError<ValidationError>()
            ? BadRequest(result.Errors)
            : CreatedAtAction(nameof(CreateVendor), result.Value);
    }
}