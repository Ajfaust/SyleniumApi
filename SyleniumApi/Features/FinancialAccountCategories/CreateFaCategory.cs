using FastEndpoints;
using FluentValidation;
using SyleniumApi.Data.Entities;
using SyleniumApi.DbContexts;
using ILogger = Serilog.ILogger;

namespace SyleniumApi.Features.FinancialAccountCategories;

public record CreateFaCategoryCommand(int LedgerId, string Name, FinancialCategoryType Type);

public record CreateFaCategoryResponse(int Id, string Name, FinancialCategoryType Type);

public class CreateFaCategoryMapper :
    Mapper<CreateFaCategoryCommand, CreateFaCategoryResponse, FinancialAccountCategory>
{
    public override Task<FinancialAccountCategory> ToEntityAsync(CreateFaCategoryCommand cmd,
        CancellationToken ct = default)
    {
        return Task.FromResult(new FinancialAccountCategory
        {
            LedgerId = cmd.LedgerId,
            FinancialAccountCategoryName = cmd.Name,
            FinancialCategoryType = cmd.Type
        });
    }

    public override Task<CreateFaCategoryResponse> FromEntityAsync(FinancialAccountCategory entity,
        CancellationToken ct = default)
    {
        return Task.FromResult(new CreateFaCategoryResponse(
            entity.FinancialAccountCategoryId,
            entity.FinancialAccountCategoryName,
            entity.FinancialCategoryType
        ));
    }
}

public class CreateFaCategoryValidator : Validator<CreateFaCategoryCommand>
{
    public CreateFaCategoryValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Type).IsInEnum();
    }
}

public class CreateFaCategoryEndpoint(SyleniumDbContext context, ILogger logger)
    : Endpoint<CreateFaCategoryCommand, CreateFaCategoryResponse, CreateFaCategoryMapper>
{
    public override void Configure()
    {
        Post("/api/fa-categories");
        AllowAnonymous();
        DontThrowIfValidationFails();
    }

    public override async Task HandleAsync(CreateFaCategoryCommand cmd, CancellationToken ct)
    {
        if (ValidationFailed)
        {
            foreach (var f in ValidationFailures)
                logger.Error("{prop} failed validation: {msg}", f.PropertyName, f.ErrorMessage);

            await SendErrorsAsync(cancellation: ct);
            return;
        }

        var category = await Map.ToEntityAsync(cmd, ct);
        await context.FinancialAccountCategories.AddAsync(category, ct);
        await context.SaveChangesAsync(ct);

        await SendMappedAsync(category, StatusCodes.Status201Created, ct);
    }
}