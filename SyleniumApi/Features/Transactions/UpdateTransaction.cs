using FluentResults;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SyleniumApi.DbContexts;
using SyleniumApi.Shared;

namespace SyleniumApi.Features.Transactions;

public record UpdateTransactionCommand(TransactionDto Dto) : IRequest<Result<UpdateTransactionResponse>>;

public record UpdateTransactionResponse(TransactionDto Dto);

public class UpdateTransactionValidator : AbstractValidator<UpdateTransactionCommand>
{
    public UpdateTransactionValidator()
    {
        RuleFor(x => x.Dto.Date).LessThanOrEqualTo(DateTime.UtcNow);
        RuleFor(x => x.Dto.Description).MaximumLength(500);
    }
}

public class UpdateTransactionHandler(SyleniumDbContext context, IValidator<UpdateTransactionCommand> validator)
    : IRequestHandler<UpdateTransactionCommand, Result<UpdateTransactionResponse>>
{
    public async Task<Result<UpdateTransactionResponse>> Handle(
        UpdateTransactionCommand command,
        CancellationToken cancellationToken
    )
    {
        var dto = command.Dto;

        var validationResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validationResult.IsValid)
            return new ValidationError("Validation failed").CausedBy(
                validationResult.Errors.Select(e => e.ErrorMessage));

        var transaction = await context.Transactions.FindAsync(dto.Id);
        if (transaction == null)
            return new EntityNotFoundError("Transaction not found");

        var accountExists = await context.FinancialAccounts.AnyAsync(a => a.FinancialAccountId == dto.AccountId);
        if (!accountExists)
            return new ValidationError("Invalid financial account Id");

        var categoryExists =
            await context.TransactionCategories.AnyAsync(c => c.TransactionCategoryId == dto.CategoryId);
        if (!categoryExists)
            return new ValidationError("Invalid category Id");

        var vendorExists = await context.Vendors.AnyAsync(v => v.VendorId == dto.VendorId);
        if (!vendorExists)
            return new ValidationError("Invalid vendor Id");

        dto.UpdateTransaction(transaction);

        context.Entry(transaction).State = EntityState.Modified;
        await context.SaveChangesAsync(cancellationToken);

        var response = transaction.ToDto();

        return Result.Ok(new UpdateTransactionResponse(response));
    }
}

public partial class TransactionsController
{
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateTransaction(int id, [FromBody] UpdateTransactionCommand command,
        ISender sender)
    {
        try
        {
            if (id != command.Dto.Id)
                return BadRequest(new ValidationError("Id in route does not match id in body"));

            var result = await sender.Send(command);

            if (result.HasError<ValidationError>())
            {
                logger.LogValidationError(result);
                return BadRequest(result.Errors);
            }

            if (result.HasError<EntityNotFoundError>())
            {
                logger.LogNotFoundError(result);
                return NotFound(result.Errors);
            }

            logger.Information($"Successfully updated transaction with Id: {id}");
            return Ok(result.Value);
        }
        catch (Exception ex)
        {
            var message = $"Unexpected error updating transaction with Id: {id}";
            logger.Error(ex, message);
            return StatusCode(StatusCodes.Status500InternalServerError, message);
        }
    }
}