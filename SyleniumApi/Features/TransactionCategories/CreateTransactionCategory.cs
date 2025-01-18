using FastEndpoints;
using FluentValidation;
using SyleniumApi.Data.Entities;
using SyleniumApi.DbContexts;
using SyleniumApi.Features.Shared;
using ILogger = Serilog.ILogger;

namespace SyleniumApi.Features.TransactionCategories;

public record CreateTransactionCategoryCommand(int LedgerId, int? ParentId, string Name);

public record CreateTransactionCategoryResponse(int Id, int? ParentId, string Name);

public class CreateTransactionCategoryMapper : Mapper<CreateTransactionCategoryCommand,
    CreateTransactionCategoryResponse, TransactionCategory>
{
    public override Task<TransactionCategory> ToEntityAsync(CreateTransactionCategoryCommand cmd,
        CancellationToken ct = default)
    {
        return Task.FromResult(new TransactionCategory
        {
            LedgerId = cmd.LedgerId,
            ParentCategoryId = cmd.ParentId,
            TransactionCategoryName = cmd.Name
        });
    }

    public override Task<CreateTransactionCategoryResponse> FromEntityAsync(TransactionCategory e,
        CancellationToken ct = default)
    {
        return Task.FromResult(new CreateTransactionCategoryResponse(e.TransactionCategoryId, e.ParentCategoryId,
            e.TransactionCategoryName));
    }
}

public class CreateTransactionCategoryValidator : Validator<CreateTransactionCategoryCommand>
{
    public CreateTransactionCategoryValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
    }
}

public class CreateTransactionCategoryEndpoint(SyleniumDbContext context, ILogger logger)
    : Endpoint<CreateTransactionCategoryCommand, CreateTransactionCategoryResponse, CreateTransactionCategoryMapper>
{
    public override void Configure()
    {
        Post("transaction-categories");
        AllowAnonymous();
    }

    public override void OnValidationFailed()
    {
        logger.LogValidationErrors(nameof(CreateTransactionCategoryEndpoint), ValidationFailures);
    }

    public override async Task HandleAsync(CreateTransactionCategoryCommand cmd, CancellationToken ct)
    {
        var category = await Map.ToEntityAsync(cmd, ct);
        await context.TransactionCategories.AddAsync(category, ct);
        await context.SaveChangesAsync(ct);

        await SendMappedAsync(category, StatusCodes.Status201Created, ct);
    }
}