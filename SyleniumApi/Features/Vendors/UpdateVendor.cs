using FluentResults;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SyleniumApi.DbContexts;
using SyleniumApi.Shared;

namespace SyleniumApi.Features.Vendors;

public record UpdateVendorCommand(int Id, string Name) : IRequest<Result<UpdateVendorResponse>>;

public record UpdateVendorResponse(int Id, string Name);

public class UpdateVendorValidator : AbstractValidator<UpdateVendorCommand>
{
    public UpdateVendorValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
    }
}

public class UpdateVendorHandler(SyleniumDbContext context, IValidator<UpdateVendorCommand> validator)
    : IRequestHandler<UpdateVendorCommand, Result<UpdateVendorResponse>>
{
    public async Task<Result<UpdateVendorResponse>> Handle(
        UpdateVendorCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(command);
        if (!validationResult.IsValid)
            return new ValidationError("One or more properties in the command is invalid")
                .CausedBy(validationResult.Errors.Select(e => new Error(e.ErrorMessage)));

        var vendor = await context.Vendors.FindAsync(command.Id);
        if (vendor == null)
            return new EntityNotFoundError("Vendor not found");

        vendor.VendorName = command.Name;
        context.Entry(vendor).State = EntityState.Modified;
        await context.SaveChangesAsync(cancellationToken);

        var response = new UpdateVendorResponse(vendor.VendorId, vendor.VendorName);

        return Result.Ok(response);
    }
}

public partial class VendorsController
{
    [HttpPut("{id:int}")]
    [ProducesResponseType<UpdateVendorResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<UpdateVendorResponse>> UpdateVendor(
        int id,
        [FromBody] UpdateVendorCommand command,
        ISender sender)
    {
        try
        {
            if (id != command.Id)
                return BadRequest("Id in the URL does not match Id in the body");
            var result = await sender.Send(command);

            if (result.HasError<EntityNotFoundError>())
            {
                logger.LogNotFoundError(result);
                return NotFound(result.Errors);
            }

            if (result.HasError<ValidationError>())
            {
                logger.LogValidationError(result);
                return BadRequest(result.Errors);
            }

            logger.Information($"Successfully updated vendor with Id: {id}");
            return Ok(result.Value);
        }
        catch (Exception ex)
        {
            var message = $"Unexpected error updating vendor with Id: {id}";
            logger.Error(ex, message);
            return StatusCode(StatusCodes.Status500InternalServerError, message);
        }
    }
}