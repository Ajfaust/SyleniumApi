using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyleniumApi.DbContexts;
using SyleniumApi.Shared;

namespace SyleniumApi.Features.Vendors;

public record DeleteVendorCommand(int Id) : IRequest<Result>;

public class DeleteVendorHandler(SyleniumDbContext context) : IRequestHandler<DeleteVendorCommand, Result>
{
    public async Task<Result> Handle(DeleteVendorCommand request, CancellationToken cancellationToken)
    {
        var vendor = await context.Vendors.FindAsync(request.Id);
        if (vendor == null)
            return new EntityNotFoundError("Vendor could not be found");

        context.Remove(vendor);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}

public partial class VendorsController
{
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, ISender sender)
    {
        var command = new DeleteVendorCommand(id);
        var result = await sender.Send(command);

        return result.HasError<EntityNotFoundError>() ? NotFound(result.Errors) : NoContent();
    }
}