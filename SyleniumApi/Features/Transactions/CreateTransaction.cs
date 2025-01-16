using FluentResults;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SyleniumApi.Data.Entities;
using SyleniumApi.DbContexts;
using SyleniumApi.Shared;

namespace SyleniumApi.Features.Transactions;

public record CreateTransactionCommand(
    int AccountId,
    int TransactionCategoryId,
    int VendorId,
    DateTime Date,
    string Description,
    decimal Inflow,
    decimal Outflow,
    bool Cleared
) : IRequest<Result<CreateTransactionResponse>>;

public record CreateTransactionResponse(int Id);

public class CreateTransactionValidator : AbstractValidator<CreateTransactionCommand>
{
    public CreateTransactionValidator()
    {
        RuleFor(x => x.Date).LessThan(DateTime.UtcNow);
        RuleFor(x => x.Description).MaximumLength(500);
    }
}

public class CreateTransactionHandler(SyleniumDbContext context, IValidator<CreateTransactionCommand> validator)
    : IRequestHandler<CreateTransactionCommand, Result<CreateTransactionResponse>>
{
    public async Task<Result<CreateTransactionResponse>> Handle(CreateTransactionCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(command);
        if (!validationResult.IsValid)
            return new ValidationError("Validation failed").CausedBy(
                validationResult.Errors.Select(e => new Error(e.ErrorMessage)));

        var accountExists =
            await context.FinancialAccounts.AnyAsync(x => x.FinancialAccountId == command.AccountId, cancellationToken);
        if (!accountExists)
            return new ValidationError("Account does not exist");

        var transactionCategoryExists =
            await context.TransactionCategories.AnyAsync(x => x.TransactionCategoryId == command.TransactionCategoryId,
                cancellationToken);
        if (!transactionCategoryExists)
            return new ValidationError("Transaction category does not exist");

        var vendorExists = await context.Vendors.AnyAsync(v => v.VendorId == command.VendorId, cancellationToken);
        if (!vendorExists)
            return new ValidationError("Vendor does not exist");

        var transaction = new Transaction
        {
            FinancialAccountId = command.AccountId,
            TransactionCategoryId = command.TransactionCategoryId,
            VendorId = command.VendorId,
            Date = command.Date,
            Description = command.Description,
            Inflow = command.Inflow,
            Outflow = command.Outflow,
            Cleared = command.Cleared
        };
        context.Transactions.Add(transaction);
        await context.SaveChangesAsync(cancellationToken);

        var response = new CreateTransactionResponse(transaction.TransactionId);

        return Result.Ok(response);
    }
}

public partial class TransactionsController
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateTransaction(CreateTransactionCommand command, ISender sender)
    {
        try
        {
            var result = await sender.Send(command);

            if (result.HasError<ValidationError>())
            {
                logger.LogValidationError(result);
                return BadRequest(result.Errors);
            }

            logger.Information($"Successfully created transaction with Id: {result.Value.Id}");
            return CreatedAtAction(nameof(GetTransaction), new { id = result.Value.Id }, result.Value);
        }
        catch (Exception ex)
        {
            const string message = "Unexpected error creating new transaction";
            logger.Error(ex, message);
            return StatusCode(StatusCodes.Status500InternalServerError, message);
        }
    }
}