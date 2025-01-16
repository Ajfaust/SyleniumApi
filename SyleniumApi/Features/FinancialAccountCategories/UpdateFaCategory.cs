using FluentResults;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyleniumApi.Data.Entities;
using SyleniumApi.DbContexts;
using SyleniumApi.Shared;

namespace SyleniumApi.Features.FinancialAccountCategories;

public record UpdateFaCategoryCommand(int Id, string Name, FinancialCategoryType Type)
    : IRequest<Result<UpdateFaCategoryResponse>>;

public record UpdateFaCategoryResponse(int Id, string Name, FinancialCategoryType Type);

public class UpdateFaCategoryValidator : AbstractValidator<UpdateFaCategoryCommand>
{
    public UpdateFaCategoryValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Type).IsInEnum();
    }
}

public class UpdateFaCategoryHandler(SyleniumDbContext context, IValidator<UpdateFaCategoryCommand> validator)
    : IRequestHandler<UpdateFaCategoryCommand, Result<UpdateFaCategoryResponse>>
{
    public async Task<Result<UpdateFaCategoryResponse>> Handle(UpdateFaCategoryCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            return new ValidationError("One or more properties are invalid");

        var category = await context.FinancialAccountCategories.FindAsync(command.Id);
        if (category is null)
            return new EntityNotFoundError($"Financial Account Category {command.Id} not found");

        category.FinancialAccountCategoryName = command.Name;
        category.FinancialCategoryType = command.Type;

        context.FinancialAccountCategories.Update(category);
        await context.SaveChangesAsync(cancellationToken);

        var response = new UpdateFaCategoryResponse(
            category.FinancialAccountCategoryId,
            category.FinancialAccountCategoryName,
            category.FinancialCategoryType
        );

        return Result.Ok(response);
    }
}

public partial class FaCategoriesController
{
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateFaCategory(int id, [FromBody] UpdateFaCategoryCommand command,
        ISender sender)
    {
        try
        {
            if (id != command.Id)
                return BadRequest("Id mismatch");

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

            logger.Information($"Successfully updated financial account category with ID: {id}");
            return Ok(result.Value);
        }
        catch (Exception ex)
        {
            var message = $"Unexpected error updating financial account category with Id: {id}";
            logger.Error(ex, message);
            return StatusCode(StatusCodes.Status500InternalServerError, message);
        }
    }
}