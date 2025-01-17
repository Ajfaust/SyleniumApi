using FastEndpoints;
using FluentValidation;
using SyleniumApi.Data.Entities;
using SyleniumApi.DbContexts;
using ILogger = Serilog.ILogger;

namespace SyleniumApi.Features.TransactionCategories;

public record UpdateTransactionCategoryCommand(int Id, int LedgerId, int? ParentId, string Name);

public record UpdateTransactionCategoryResponse(int Id, int? ParentId, string Name);

public class UpdateTransactionCategoryMapper : Mapper<UpdateTransactionCategoryCommand,
    UpdateTransactionCategoryResponse, TransactionCategory>
{
    public override UpdateTransactionCategoryResponse FromEntity(TransactionCategory e)
    {
        return new UpdateTransactionCategoryResponse(e.TransactionCategoryId, e.ParentCategoryId,
            e.TransactionCategoryName);
    }
}

public class UpdateTransactionCategoryValidator : AbstractValidator<UpdateTransactionCategoryCommand>
{
    public UpdateTransactionCategoryValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
    }
}

public class UpdateTransactionCategoryEndpoint(SyleniumDbContext context, ILogger logger)
    : Endpoint<UpdateTransactionCategoryCommand, UpdateTransactionCategoryResponse, UpdateTransactionCategoryMapper>
{
    public override void Configure()
    {
        Put("/api/transaction-categories/{Id:int}");
        AllowAnonymous();
        DontThrowIfValidationFails();
    }

    public override async Task HandleAsync(UpdateTransactionCategoryCommand cmd, CancellationToken ct)
    {
        if (ValidationFailed)
        {
            logger.Error("Validation failed for UpdateTransactionCategory");
            foreach (var f in ValidationFailures)
                logger.Error("{0}: {1}", f.PropertyName, f.ErrorMessage);

            await SendErrorsAsync(cancellation: ct);
            return;
        }

        var category = await context.TransactionCategories.FindAsync(cmd.Id);
        if (category is null)
        {
            logger.Error("TransactionCategory with id {0} not found", cmd.Id);
            await SendNotFoundAsync(ct);
            return;
        }

        category.ParentCategoryId = cmd.ParentId;
        category.TransactionCategoryName = cmd.Name;

        context.TransactionCategories.Update(category);
        await context.SaveChangesAsync(ct);

        await SendMappedAsync(category);
    }
}