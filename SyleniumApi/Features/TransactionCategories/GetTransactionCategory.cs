using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyleniumApi.DbContexts;
using SyleniumApi.Shared;

namespace SyleniumApi.Features.TransactionCategories;

public record GetTransactionCategoryRequest(int Id) : IRequest<Result<GetTransactionCategoryResponse>>;

public record GetTransactionCategoryResponse(int Id, int? ParentId, string Name);

public class GetTransactionCategoryHandler(SyleniumDbContext context)
    : IRequestHandler<GetTransactionCategoryRequest, Result<GetTransactionCategoryResponse>>
{
    public async Task<Result<GetTransactionCategoryResponse>> Handle(
        GetTransactionCategoryRequest request, CancellationToken cancellationToken)
    {
        var transactionCategory = await context.TransactionCategories.FindAsync(request.Id);
        if (transactionCategory is null)
            return new EntityNotFoundError("Transaction category not found");

        var result = new GetTransactionCategoryResponse(transactionCategory.TransactionCategoryId,
            transactionCategory.ParentCategoryId, transactionCategory.TransactionCategoryName);

        return Result.Ok(result);
    }
}

public partial class TransactionCategoriesController
{
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<GetTransactionCategoryResponse>> GetTransactionCategory(
        int id, ISender sender)
    {
        try
        {
            var request = new GetTransactionCategoryRequest(id);
            var result = await sender.Send(request);

            if (result.HasError<EntityNotFoundError>())
            {
                logger.LogNotFoundError(result);
                return NotFound(result.Errors);
            }

            logger.Information($"Successfully retrieved transaction category with Id: {id}");
            return Ok(result.Value);
        }
        catch (Exception ex)
        {
            var message = $"Unexpected error retrieving transaction category with Id: {id}";
            logger.Error(ex, message);
            return StatusCode(StatusCodes.Status500InternalServerError, message);
        }
    }
}