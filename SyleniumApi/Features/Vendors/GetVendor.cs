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
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<GetVendorResponse>> GetVendor(int id, ISender sender)
    {
        try
        {
            var result = await sender.Send(new GetVendorQuery(id));

            if (result.HasError<EntityNotFoundError>())
            {
                logger.LogNotFoundError(result);
                return NotFound(result.Errors);
            }

            logger.Information($"Successfully retrieved vendor with Id: {id}");
            return Ok(result.Value);
        }
        catch (Exception ex)
        {
            var message = $"Unexpected error retrieving vendor with Id: {id}";
            logger.Error(ex, message);
            return StatusCode(StatusCodes.Status500InternalServerError, message);
        }
    }
}