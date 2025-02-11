using FastEndpoints;
using FluentValidation;
using SyleniumApi.Data.Entities;
using SyleniumApi.DbContexts;
using SyleniumApi.Features.Shared;
using ILogger = Serilog.ILogger;

namespace SyleniumApi.Features.TransactionCategories;

public record CreateTransactionCategoryCommand(
    int LedgerId,
    int? ParentId,
    string Name,
    List<CreateTransactionCategoryCommand>? Subcategories);

public record CreateTransactionCategoryResponse(int Id, int? ParentId, string Name);

public class CreateTransactionCategoryValidator : Validator<CreateTransactionCategoryCommand>
{
    public CreateTransactionCategoryValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
    }
}

public class CreateTransactionCategoryEndpoint(SyleniumDbContext context, ILogger logger)
    : Endpoint<CreateTransactionCategoryCommand, CreateTransactionCategoryResponse>
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
        var category = cmd.ToEntity();
        await context.TransactionCategories.AddAsync(category, ct);
        await context.SaveChangesAsync(true, ct);

        // Create subcategories if necessary, using the newly created category as parent
        if (cmd.Subcategories is { Count: > 0 })
        {
            var subCats = cmd.Subcategories.Select(c =>
            {
                var subCat = c.ToEntity();
                subCat.ParentCategoryId = category.Id;

                return subCat;
            }).ToList();

            await context.AddRangeAsync(subCats, ct);
            await context.SaveChangesAsync(ct);
        }

        await SendAsync(category.ToCreateResponse(), StatusCodes.Status201Created, ct);
    }
}

public static partial class TransactionCategoryMappers
{
    public static TransactionCategory ToEntity(this CreateTransactionCategoryCommand cmd)
    {
        return new TransactionCategory
        {
            LedgerId = cmd.LedgerId,
            ParentCategoryId = cmd.ParentId,
            Name = cmd.Name
        };
    }

    public static CreateTransactionCategoryResponse ToCreateResponse(this TransactionCategory category)
    {
        return new CreateTransactionCategoryResponse(category.Id, category.ParentCategoryId, category.Name);
    }
}