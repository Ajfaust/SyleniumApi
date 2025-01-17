using FastEndpoints;
using FluentValidation;
using SyleniumApi.Data.Entities;
using SyleniumApi.DbContexts;
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
        DontThrowIfValidationFails();
    }

    public override async Task HandleAsync(CreateTransactionCommand cmd, CancellationToken ct)
    {
        if (ValidationFailed)
        {
            logger.Error("Validation failed for CreateTransaction");
            foreach (var f in ValidationFailures)
                logger.Error("{0}: {1}", f.PropertyName, f.ErrorMessage);

            await SendErrorsAsync(cancellation: ct);
            return;
        }

        var transaction = await Map.ToEntityAsync(cmd, ct);
        await context.Transactions.AddAsync(transaction, ct);
        await context.SaveChangesAsync(ct);

        await SendMappedAsync(transaction, StatusCodes.Status201Created, ct);
    }
}