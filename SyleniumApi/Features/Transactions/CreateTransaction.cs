using FastEndpoints;
using FluentValidation;
using SyleniumApi.Data.Entities;
using SyleniumApi.DbContexts;
using SyleniumApi.Features.Shared;
using ILogger = Serilog.ILogger;

namespace SyleniumApi.Features.Transactions;

public record CreateTransactionCommand(TransactionDto Dto);

public record CreateTransactionResponse(TransactionDto Dto);

public class CreateTransactionMapper : Mapper<CreateTransactionCommand, CreateTransactionResponse, Transaction>
{
    public override Task<Transaction> ToEntityAsync(CreateTransactionCommand cmd, CancellationToken ct = default)
    {
        var dto = cmd.Dto;
        return Task.FromResult(new Transaction
        {
            FinancialAccountId = dto.AccountId,
            TransactionCategoryId = dto.CategoryId,
            VendorId = dto.VendorId,
            Date = dto.Date,
            Description = dto.Description,
            Inflow = dto.Inflow,
            Outflow = dto.Outflow,
            Cleared = dto.Cleared
        });
    }

    public override Task<CreateTransactionResponse> FromEntityAsync(Transaction e, CancellationToken ct = default)
    {
        var dto = new TransactionDto
        {
            Id = e.TransactionId,
            AccountId = e.FinancialAccountId,
            CategoryId = e.TransactionCategoryId,
            Date = e.Date,
            Description = e.Description ?? string.Empty,
            Inflow = e.Inflow,
            Outflow = e.Outflow,
            Cleared = e.Cleared
        };

        return Task.FromResult(new CreateTransactionResponse(dto));
    }
}

public class CreateTransactionValidator : Validator<CreateTransactionCommand>
{
    public CreateTransactionValidator()
    {
        RuleFor(x => x.Dto.Date).LessThanOrEqualTo(DateTime.UtcNow);
        RuleFor(x => x.Dto.Description).MaximumLength(500);
    }
}

public class CreateTransactionEndpoint(SyleniumDbContext context, ILogger logger)
    : Endpoint<CreateTransactionCommand, CreateTransactionResponse, CreateTransactionMapper>
{
    public override void Configure()
    {
        Post("/api/transactions");
        AllowAnonymous();
    }

    public override void OnBeforeValidate(CreateTransactionCommand cmd)
    {
        // Add validation checks for the appropriate FKs existing
        var accountExists =
            context.FinancialAccounts.Any(a => a.FinancialAccountId == cmd.Dto.AccountId);
        if (!accountExists)
            AddError($"AccountId {cmd.Dto.AccountId} does not exist");

        var categoryExists =
            context.TransactionCategories.Any(c => c.TransactionCategoryId == cmd.Dto.CategoryId);
        if (!categoryExists)
            AddError($"CategoryId {cmd.Dto.CategoryId} does not exist");

        var vendorExists = context.Vendors.Any(v => v.VendorId == cmd.Dto.VendorId);
        if (!vendorExists)
            AddError($"VendorId {cmd.Dto.VendorId} does not exist");
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