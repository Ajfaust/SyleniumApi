using FluentResults;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyleniumApi.Data.Entities;
using SyleniumApi.DbContexts;
using SyleniumApi.Shared;

namespace SyleniumApi.Features.FinancialAccounts;

public record CreateFinancialAccountCommand(string Name, int LedgerId, int FaCategoryId)
    : IRequest<Result<CreateFinancialAccountResponse>>;

public record CreateFinancialAccountResponse(int Id, string Name, int LedgerId, int FaCategoryId);

public class CreateFinancialAccountValidator : AbstractValidator<CreateFinancialAccountCommand>
{
    public CreateFinancialAccountValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.LedgerId).NotEmpty();
        RuleFor(x => x.FaCategoryId).NotEmpty();
    }
}

public class
    CreateFinancialAccountHandler(SyleniumDbContext context, IValidator<CreateFinancialAccountCommand> validator)
    : IRequestHandler<CreateFinancialAccountCommand,
        Result<CreateFinancialAccountResponse>>
{
    public async Task<Result<CreateFinancialAccountResponse>> Handle(CreateFinancialAccountCommand request,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return new ValidationError("One or more properties are invalid");

        var entity = new FinancialAccount
        {
            FinancialAccountName = request.Name,
            LedgerId = request.LedgerId,
            FinancialAccountCategoryId = request.FaCategoryId
        };

        context.FinancialAccounts.Add(entity);

        await context.SaveChangesAsync(cancellationToken);

        var response = new CreateFinancialAccountResponse(
            entity.FinancialAccountId,
            entity.FinancialAccountName,
            entity.LedgerId,
            entity.FinancialAccountCategoryId
        );

        return Result.Ok(response);
    }
}

public partial class FinancialAccountsController
{
    // POST /api/financial-accounts
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<CreateFinancialAccountResponse>> CreateFinancialAccount(
        [FromBody] CreateFinancialAccountCommand command,
        ISender sender)
    {
        var result = await sender.Send(command);

        return result.HasError<ValidationError>()
            ? BadRequest(result.Errors)
            : CreatedAtAction("GetFinancialAccount", new { id = result.Value.Id }, result.Value);
    }
}