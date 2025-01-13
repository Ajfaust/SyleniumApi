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
    public async Task<IActionResult> GetFaCategory(int id, ISender sender)
    {
        var request = new GetFaCategoryRequest(id);
        var result = await sender.Send(request);

        return result.HasError<EntityNotFoundError>() ? NotFound(result.Errors) : Ok(result.Value);
    }
}