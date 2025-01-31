using FastEndpoints;
using FluentValidation;
using SyleniumApi.Data.Entities;
using SyleniumApi.DbContexts;
using SyleniumApi.Features.Shared;
using ILogger = Serilog.ILogger;

namespace SyleniumApi.Features.Transactions;

public record CreateTransactionCommand(
    int AccountId,
    int CategoryId,
    int VendorId,
    DateTime Date,
    string Description,
    decimal Inflow,
    decimal Outflow,
    bool Cleared);

public record CreateTransactionResponse(
    int Id,
    int AccountId,
    int CategoryId,
    int VendorId,
    DateTime Date,
    string Description,
    decimal Inflow,
    decimal Outflow,
    bool Cleared);

public class CreateTransactionMapper : Mapper<CreateTransactionCommand, CreateTransactionResponse, Transaction>
{
    public override Task<Transaction> ToEntityAsync(CreateTransactionCommand cmd, CancellationToken ct = default)
    {
        return Task.FromResult(new Transaction
        {
            FinancialAccountId = cmd.AccountId,
            TransactionCategoryId = cmd.CategoryId,
            VendorId = cmd.VendorId,
            Date = cmd.Date,
            Description = cmd.Description,
            Inflow = cmd.Inflow,
            Outflow = cmd.Outflow,
            Cleared = cmd.Cleared
        });
    }

    public override Task<CreateTransactionResponse> FromEntityAsync(Transaction e, CancellationToken ct = default)
    {
        return Task.FromResult(new CreateTransactionResponse(
            e.Id,
            e.FinancialAccountId,
            e.TransactionCategoryId,
            e.VendorId,
            e.Date,
            e.Description ?? string.Empty,
            e.Inflow,
            e.Outflow,
            e.Cleared
        ));
    }
}

public class CreateTransactionValidator : Validator<CreateTransactionCommand>
{
    public CreateTransactionValidator()
    {
        RuleFor(x => x.Date).LessThanOrEqualTo(DateTime.UtcNow);
        RuleFor(x => x.Description).MaximumLength(500);
    }
}

public class CreateTransactionEndpoint(SyleniumDbContext context, ILogger logger)
    : Endpoint<CreateTransactionCommand, CreateTransactionResponse, CreateTransactionMapper>
{
    public override void Configure()
    {
        Post("transactions");
        AllowAnonymous();
    }

    public override void OnBeforeValidate(CreateTransactionCommand cmd)
    {
        // Add validation checks for the appropriate FKs existing
        var accountExists =
            context.FinancialAccounts.Any(a => a.Id == cmd.AccountId);
        if (!accountExists)
            AddError($"AccountId {cmd.AccountId} does not exist");

        var categoryExists =
            context.TransactionCategories.Any(c => c.Id == cmd.CategoryId);
        if (!categoryExists)
            AddError($"CategoryId {cmd.CategoryId} does not exist");

        var vendorExists = context.Vendors.Any(v => v.Id == cmd.VendorId);
        if (!vendorExists)
            AddError($"VendorId {cmd.VendorId} does not exist");
    }

    public override void OnValidationFailed()
    {
        logger.LogValidationErrors(nameof(CreateTransactionEndpoint), ValidationFailures);
    }

    public override async Task HandleAsync(CreateTransactionCommand cmd, CancellationToken ct)
    {
        var transaction = await Map.ToEntityAsync(cmd, ct);
        await context.Transactions.AddAsync(transaction, ct);
        await context.SaveChangesAsync(ct);

        await SendMappedAsync(transaction, StatusCodes.Status201Created, ct);
    }
}