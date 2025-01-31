using FastEndpoints;
using FluentValidation;
using SyleniumApi.Data.Entities;
using SyleniumApi.DbContexts;
using SyleniumApi.Features.Shared;
using ILogger = Serilog.ILogger;

namespace SyleniumApi.Features.Transactions;

public record UpdateTransactionCommand(
    int Id,
    int AccountId,
    int CategoryId,
    int VendorId,
    DateTime Date,
    string Description,
    decimal Inflow,
    decimal Outflow,
    bool Cleared);

public record UpdateTransactionResponse(
    int Id,
    int AccountId,
    int CategoryId,
    int VendorId,
    DateTime Date,
    string Description,
    decimal Inflow,
    decimal Outflow,
    bool Cleared);

public class UpdateTransactionMapper : Mapper<UpdateTransactionCommand, UpdateTransactionResponse, Transaction>
{
    public override Task<UpdateTransactionResponse> FromEntityAsync(Transaction e, CancellationToken ct = default)
    {
        return Task.FromResult(new UpdateTransactionResponse(
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

public class UpdateTransactionValidator : Validator<UpdateTransactionCommand>
{
    public UpdateTransactionValidator()
    {
        RuleFor(x => x.Date).LessThanOrEqualTo(DateTime.UtcNow);
        RuleFor(x => x.Description).MaximumLength(500);
    }
}

public class UpdateTransactionEndpoint(SyleniumDbContext context, ILogger logger)
    : Endpoint<UpdateTransactionCommand, UpdateTransactionResponse, UpdateTransactionMapper>
{
    public override void Configure()
    {
        Put("transactions/{Id:int}");
        Description(b => b.Produces(404));
        AllowAnonymous();
    }

    public override void OnBeforeValidate(UpdateTransactionCommand cmd)
    {
        // Add validation checks for the appropriate FKs existing
        var accountExists = context.FinancialAccounts.Any(a => a.Id == cmd.AccountId);
        if (!accountExists)
            AddError($"AccountId {cmd.AccountId} does not exist");

        var categoryExists = context.TransactionCategories.Any(c => c.Id == cmd.CategoryId);
        if (!categoryExists)
            AddError($"CategoryId {cmd.CategoryId} does not exist");

        var vendorExists = context.Vendors.Any(v => v.Id == cmd.VendorId);
        if (!vendorExists)
            AddError($"VendorId {cmd.VendorId} does not exist");
    }

    public override void OnValidationFailed()
    {
        logger.LogValidationErrors(nameof(UpdateTransactionEndpoint), ValidationFailures);
    }

    public override async Task HandleAsync(UpdateTransactionCommand cmd, CancellationToken ct)
    {
        var transaction = await context.Transactions.FindAsync(cmd.Id, ct);
        if (transaction is null)
        {
            logger.Error("Transaction with id {0} not found", cmd.Id);
            await SendNotFoundAsync(ct);
            return;
        }

        transaction.FinancialAccountId = cmd.AccountId;
        transaction.TransactionCategoryId = cmd.CategoryId;
        transaction.VendorId = cmd.VendorId;
        transaction.Date = cmd.Date;
        transaction.Description = cmd.Description;
        transaction.Inflow = cmd.Inflow;
        transaction.Outflow = cmd.Outflow;
        transaction.Cleared = cmd.Cleared;

        context.Transactions.Update(transaction);
        await context.SaveChangesAsync(ct);

        await SendMappedAsync(transaction, ct: ct);
    }
}