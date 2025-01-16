using FluentResults;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyleniumApi.DbContexts;
using SyleniumApi.Shared;

namespace SyleniumApi.Features.TransactionCategories;

public record UpdateTransactionCategoryCommand(int Id, int? ParentId, string Name)
    : IRequest<Result<UpdateTransactionCategoryResponse>>;

public record UpdateTransactionCategoryResponse(int Id, int? ParentId, string Name);

public class UpdateTransactionCategoryValidator : AbstractValidator<UpdateTransactionCategoryCommand>
{
    public UpdateTransactionCategoryValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
    }
}

public class UpdateTransactionCategoryHandler(
    SyleniumDbContext context,
    IValidator<UpdateTransactionCategoryCommand> validator)
    : IRequestHandler<UpdateTransactionCategoryCommand, Result<UpdateTransactionCategoryResponse>>
{
    public async Task<Result<UpdateTransactionCategoryResponse>> Handle(UpdateTransactionCategoryCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(command);
        if (!validationResult.IsValid)
            return new ValidationError(errors: validationResult.Errors);

        var transactionCategory = await context.TransactionCategories.FindAsync(command.Id);
        if (transactionCategory == null)
            return new EntityNotFoundError("Transaction category not found");

        if (transactionCategory.ParentCategoryId != command.ParentId && command.ParentId != null)
        {
            var parentCategory = await context.TransactionCategories.FindAsync(command.ParentId);
            if (parentCategory == null)
                return new ValidationError("Parent Category not found");
        }

        transactionCategory.ParentCategoryId = command.ParentId;
        transactionCategory.TransactionCategoryName = command.Name;

        await context.SaveChangesAsync(cancellationToken);

        var response = new UpdateTransactionCategoryResponse(transactionCategory.TransactionCategoryId,
            transactionCategory.ParentCategoryId, transactionCategory.TransactionCategoryName);

        return Result.Ok(response);
    }
}

public partial class TransactionCategoriesController
{
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateTransactionCategory(int id,
        [FromBody] UpdateTransactionCategoryCommand command, ISender sender)
    {
        try
        {
            if (id != command.Id)
                return BadRequest("Id from route does not match Id in body");

            var result = await sender.Send(command);

            if (result.HasError<EntityNotFoundError>())
            {
                logger.LogNotFoundError(result);
                return NotFound(result.Errors);
            }

            if (result.HasError<ValidationError>())
                return BadRequest(result.Errors);

            logger.Information($"Successfully updated transaction category with Id: {id}");
            return Ok(result.Value);
        }
        catch (Exception ex)
        {
            var message = $"Unexpected error updating transaction category with Id: {id}";
            logger.Error(ex, message);
            return StatusCode(StatusCodes.Status500InternalServerError, message);
        }
    }
}