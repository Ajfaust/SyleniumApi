using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyleniumApi.Data.Entities;
using SyleniumApi.DbContexts;
using SyleniumApi.Shared;

namespace SyleniumApi.Features.FinancialAccountCategories;

public record GetFaCategoryRequest(int Id) : IRequest<Result<GetFaCategoryResponse>>;

public record GetFaCategoryResponse(int Id, string Name, FinancialCategoryType Type);

public class GetFaCategoryHandler(SyleniumDbContext context)
    : IRequestHandler<GetFaCategoryRequest, Result<GetFaCategoryResponse>>
{
    public async Task<Result<GetFaCategoryResponse>> Handle(GetFaCategoryRequest request,
        CancellationToken cancellationToken)
    {
        var faCategory = await context.FinancialAccountCategories.FindAsync(request.Id);
        if (faCategory is null)
            return new EntityNotFoundError($"Financial Account Category {request.Id} not found");

        var response = new GetFaCategoryResponse(faCategory.FinancialAccountCategoryId,
            faCategory.FinancialAccountCategoryName, faCategory.FinancialCategoryType);

        return Result.Ok(response);
    }
}

public partial class FaCategoriesController
{
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetFaCategory(int id, ISender sender)
    {
        try
        {
            var request = new GetFaCategoryRequest(id);
            var result = await sender.Send(request);

            if (result.HasError<EntityNotFoundError>())
            {
                logger.LogNotFoundError(result);
                return NotFound(result.Errors);
            }

            logger.Information($"Successfully retrieved financial account category with Id: {id}");
            return Ok(result.Value);
        }
        catch (Exception ex)
        {
            var message = $"Unexpected error retrieving financial account category with Id: {id}";
            logger.Error(ex, message);
            return StatusCode(StatusCodes.Status500InternalServerError, message);
        }
    }
}