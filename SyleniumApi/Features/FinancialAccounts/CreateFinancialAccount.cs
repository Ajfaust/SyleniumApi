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
            return new ValidationError(errors: validationResult.Errors);

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
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<CreateFinancialAccountResponse>> CreateFinancialAccount(
        [FromBody] CreateFinancialAccountCommand command,
        ISender sender)
    {
        try
        {
            var result = await sender.Send(command);

            if (result.HasError<ValidationError>())
            {
                logger.LogValidationError(result);
                return BadRequest(result.Errors);
            }

            logger.Information($"Successfully created financial account with Id: {result.Value.Id}");
            return CreatedAtAction("GetFinancialAccount", new { id = result.Value.Id }, result.Value);
        }
        catch (Exception ex)
        {
            const string message = "Unexpected error creating new financial account";
            logger.Error(ex, message);
            return StatusCode(StatusCodes.Status500InternalServerError, message);
        }
    }
}