using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyleniumApi.DbContexts;
using SyleniumApi.Shared;

namespace SyleniumApi.Features.Vendors;

public record GetVendorQuery(int Id) : IRequest<Result<GetVendorResponse>>;

public record GetVendorResponse(int Id, string Name);

public class GetVendorHandler(SyleniumDbContext context) : IRequestHandler<GetVendorQuery, Result<GetVendorResponse>>
{
    public async Task<Result<GetVendorResponse>> Handle(GetVendorQuery query, CancellationToken cancellationToken)
    {
        var entity = await context.Vendors.FindAsync(query.Id);

        if (entity is null)
            return new EntityNotFoundError("Vendor not found");

        var response = new GetVendorResponse(entity.VendorId, entity.VendorName);
        return Result.Ok(response);
    }
}

public partial class VendorsController
{
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<GetVendorResponse>> GetVendor(int id, ISender sender)
    {
        var result = await sender.Send(new GetVendorQuery(id));

        return result.HasError<EntityNotFoundError>() ? NotFound(result.Errors) : Ok(result.Value);
    }
}