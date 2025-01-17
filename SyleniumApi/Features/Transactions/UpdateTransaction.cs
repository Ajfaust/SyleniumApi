using FastEndpoints;
using FluentValidation;
using SyleniumApi.Data.Entities;
using SyleniumApi.DbContexts;
using ILogger = Serilog.ILogger;

namespace SyleniumApi.Features.Transactions;

public record UpdateTransactionCommand(TransactionDto Dto);

public record UpdateTransactionResponse(TransactionDto Dto);

public class UpdateTransactionMapper : Mapper<UpdateTransactionCommand, UpdateTransactionResponse, Transaction>
{
    public override Transaction ToEntity(UpdateTransactionCommand cmd)
    {
        var dto = cmd.Dto;
        return new Transaction
        {
            FinancialAccountId = dto.AccountId,
            TransactionCategoryId = dto.CategoryId,
            VendorId = dto.VendorId,
            Date = dto.Date,
            Description = dto.Description,
            Inflow = dto.Inflow,
            Outflow = dto.Outflow,
            Cleared = dto.Cleared
        };
    }

    public override UpdateTransactionResponse FromEntity(Transaction e)
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

        return new UpdateTransactionResponse(dto);
    }
}

public class UpdateTransactionValidator : Validator<UpdateTransactionCommand>
{
    public UpdateTransactionValidator()
    {
        RuleFor(x => x.Dto.Date).LessThanOrEqualTo(DateTime.UtcNow);
        RuleFor(x => x.Dto.Description).MaximumLength(500);
    }
}

public class UpdateTransactionEndpoint(SyleniumDbContext context, ILogger logger)
    : Endpoint<UpdateTransactionCommand, UpdateTransactionResponse, UpdateTransactionMapper>
{
    public override void Configure()
    {
        Put("/api/transactions/{Id:int}");
        AllowAnonymous();
        DontThrowIfValidationFails();
    }

    public override async Task HandleAsync(UpdateTransactionCommand cmd, CancellationToken ct)
    {
        if (ValidationFailed)
        {
            logger.Error("Validation failed for UpdateTransaction");
            foreach (var f in ValidationFailures)
                logger.Error("{0}: {1}", f.PropertyName, f.ErrorMessage);

            await SendErrorsAsync(cancellation: ct);
            return;
        }

        var transaction = await context.Transactions.FindAsync(cmd.Dto.Id, ct);
        if (transaction is null)
        {
            logger.Error("Transaction with id {0} not found", cmd.Dto.Id);
            await SendNotFoundAsync(ct);
            return;
        }

        transaction.FinancialAccountId = cmd.Dto.AccountId;
        transaction.TransactionCategoryId = cmd.Dto.CategoryId;
        transaction.VendorId = cmd.Dto.VendorId;
        transaction.Date = cmd.Dto.Date;
        transaction.Description = cmd.Dto.Description;
        transaction.Inflow = cmd.Dto.Inflow;
        transaction.Outflow = cmd.Dto.Outflow;
        transaction.Cleared = cmd.Dto.Cleared;

        context.Transactions.Update(transaction);
        await context.SaveChangesAsync(ct);

        await SendMappedAsync(transaction, ct: ct);
    }
}