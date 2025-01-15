using FluentResults;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyleniumApi.Data.Entities;
using SyleniumApi.DbContexts;
using SyleniumApi.Shared;

namespace SyleniumApi.Features.TransactionCategories;

public record CreateTransactionCategoryCommand(int LedgerId, int? ParentId, string Name)
    : IRequest<Result<CreateTransactionCategoryResponse>>;

public record CreateTransactionCategoryResponse(int Id, int? ParentId, string Name);

public class CreateTransactionCategoryValidator : AbstractValidator<CreateTransactionCategoryCommand>
{
    public CreateTransactionCategoryValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
    }
}

public class CreateTransactionCategoryHandler(
    SyleniumDbContext context,
    IValidator<CreateTransactionCategoryCommand> validator)
    : IRequestHandler<CreateTransactionCategoryCommand, Result<CreateTransactionCategoryResponse>>
{
    public async Task<Result<CreateTransactionCategoryResponse>> Handle(CreateTransactionCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return new ValidationError("Error with request validation:").CausedBy(
                validationResult.Errors.Select(e => e.ErrorMessage));

        if (request.ParentId is not null)
        {
            var parent = await context.TransactionCategories.FindAsync(request.ParentId, cancellationToken);
            if (parent is null)
                return new EntityNotFoundError("Parent category not found.");
        }

        var transactionCategory = new TransactionCategory
        {
            LedgerId = request.LedgerId,
            ParentCategoryId = request.ParentId,
            TransactionCategoryName = request.Name
        };

        context.TransactionCategories.Add(transactionCategory);
        await context.SaveChangesAsync(cancellationToken);

        var response = new CreateTransactionCategoryResponse
        (
            transactionCategory.TransactionCategoryId,
            transactionCategory.ParentCategoryId,
            transactionCategory.TransactionCategoryName
        );

        return Result.Ok(response);
    }
}

public partial class TransactionCategoriesController
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CreateTransactionCategoryResponse>> CreateTransactionCategory(
        [FromBody] CreateTransactionCategoryCommand request, ISender sender)
    {
        var result = await sender.Send(request);
        return result.HasError<ValidationError>()
            ? BadRequest(result.Errors)
            : CreatedAtAction(nameof(GetTransactionCategory), new { id = result.Value.Id }, result.Value);
    }
}