using FluentResults;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyleniumApi.Data.Entities;
using SyleniumApi.DbContexts;
using SyleniumApi.Shared;

namespace SyleniumApi.Features.FinancialAccountCategories;

public record CreateFaCategoryCommand(string Name, FinancialCategoryType Type)
    : IRequest<Result<CreateFaCategoryResponse>>;

public record CreateFaCategoryResponse(int Id, string Name, FinancialCategoryType Type);

public class CreateFaCategoryValidator : AbstractValidator<CreateFaCategoryCommand>
{
    public CreateFaCategoryValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Type).IsInEnum();
    }
}

public class CreateFaCategoryHandler(SyleniumDbContext context, IValidator<CreateFaCategoryCommand> validator)
    : IRequestHandler<CreateFaCategoryCommand, Result<CreateFaCategoryResponse>>
{
    public async Task<Result<CreateFaCategoryResponse>> Handle(CreateFaCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return new ValidationError("One or more properties are invalid");

        var entity = new FinancialAccountCategory
        {
            FinancialAccountCategoryName = request.Name,
            FinancialCategoryType = request.Type
        };

        context.FinancialAccountCategories.Add(entity);

        await context.SaveChangesAsync(cancellationToken);

        var response = new CreateFaCategoryResponse(
            entity.FinancialAccountCategoryId,
            entity.FinancialAccountCategoryName,
            entity.FinancialCategoryType
        );

        return Result.Ok(response);
    }
}

public partial class FaCategoriesController
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CreateFaCategoryResponse>> CreateFaCategory(
        [FromBody] CreateFaCategoryCommand command,
        ISender sender)
    {
        var result = await sender.Send(command);

        return result.HasError<ValidationError>()
            ? BadRequest(result.Errors)
            : CreatedAtAction("GetFaCategory", new { id = result.Value.Id }, result.Value);
    }
}