using FluentResults;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SyleniumApi.DbContexts;
using SyleniumApi.Shared;

namespace SyleniumApi.Features.FinancialAccounts;

public record UpdateFinancialAccountRequest(int Id, string Name)
    : IRequest<Result<UpdateFinancialAccountResponse>>;

public record UpdateFinancialAccountResponse(int Id, string Name);

public class UpdateFinancialAccountValidator : AbstractValidator<UpdateFinancialAccountRequest>
{
    public UpdateFinancialAccountValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
    }
}

public class UpdateFinancialAccountHandler(
    SyleniumDbContext context,
    IValidator<UpdateFinancialAccountRequest> validator)
    : IRequestHandler<UpdateFinancialAccountRequest, Result<UpdateFinancialAccountResponse>>
{
    public async Task<Result<UpdateFinancialAccountResponse>> Handle(UpdateFinancialAccountRequest request,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return new ValidationError(errors: validationResult.Errors);

        var entity = await context.FinancialAccounts.FindAsync(request.Id);
        if (entity == null)
            return new EntityNotFoundError("Financial account not found");

        entity.FinancialAccountName = request.Name;

        context.Entry(entity).State = EntityState.Modified;
        await context.SaveChangesAsync(cancellationToken);

        var response = new UpdateFinancialAccountResponse(entity.FinancialAccountId, entity.FinancialAccountName);

        return Result.Ok(response);
    }
}

public partial class FinancialAccountsController
{
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateFinancialAccount(int id, [FromBody] UpdateFinancialAccountRequest request,
        ISender sender)
    {
        try
        {
            if (id != request.Id)
                return BadRequest("Id in request body does not match id in route");

            var result = await sender.Send(request);

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

            logger.Information($"Successfully updated financial account with Id: {id}");
            return Ok(result.Value);
        }
        catch (Exception ex)
        {
            var message = $"Unexpected error updating financial account with Id: {id}";
            logger.Error(ex, message);
            return StatusCode(StatusCodes.Status500InternalServerError, message);
        }
    }
}