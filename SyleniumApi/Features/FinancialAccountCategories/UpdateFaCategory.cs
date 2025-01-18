using FastEndpoints;
using FluentValidation;
using SyleniumApi.Data.Entities;
using SyleniumApi.DbContexts;
using SyleniumApi.Features.Shared;
using ILogger = Serilog.ILogger;

namespace SyleniumApi.Features.FinancialAccountCategories;

public record UpdateFaCategoryCommand(int Id, int LedgerId, string Name, FinancialCategoryType Type);

public record UpdateFaCategoryResponse(int Id, string Name, FinancialCategoryType Type);

public class
    UpdateFaCategoryMapper : Mapper<UpdateFaCategoryCommand, UpdateFaCategoryResponse, FinancialAccountCategory>
{
    public override Task<FinancialAccountCategory> ToEntityAsync(UpdateFaCategoryCommand cmd,
        CancellationToken ct = default)
    {
        return Task.FromResult(new FinancialAccountCategory
        {
            LedgerId = cmd.LedgerId,
            FinancialAccountCategoryId = cmd.Id,
            FinancialAccountCategoryName = cmd.Name,
            FinancialCategoryType = cmd.Type
        });
    }

    public override Task<UpdateFaCategoryResponse> FromEntityAsync(FinancialAccountCategory fac,
        CancellationToken ct = default)
    {
        return Task.FromResult(new UpdateFaCategoryResponse(fac.FinancialAccountCategoryId,
            fac.FinancialAccountCategoryName,
            fac.FinancialCategoryType));
    }
}

public class UpdateFaCategoryValidator : Validator<UpdateFaCategoryCommand>
{
    public UpdateFaCategoryValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Type).IsInEnum();
    }
}

public class UpdateFaCategoryEndpoint(SyleniumDbContext context, ILogger logger)
    : Endpoint<UpdateFaCategoryCommand, UpdateFaCategoryResponse, UpdateFaCategoryMapper>
{
    public override void Configure()
    {
        Put("/api/fa-categories/{Id:int}");
        AllowAnonymous();
    }

    public override void OnValidationFailed()
    {
        logger.LogValidationErrors(nameof(UpdateFaCategoryEndpoint), ValidationFailures);
    }

    public override async Task HandleAsync(UpdateFaCategoryCommand cmd, CancellationToken ct)
    {
        var category = await context.FinancialAccountCategories.FindAsync(cmd.Id, ct);
        if (category == null)
        {
            logger.Error("Financial account category with id {id} not found", cmd.Id);
            await SendNotFoundAsync(ct);
            return;
        }

        // Only change allowed fields
        category.FinancialAccountCategoryName = cmd.Name;
        category.FinancialCategoryType = cmd.Type;

        context.FinancialAccountCategories.Update(category);
        await context.SaveChangesAsync(ct);

        await SendMappedAsync(category, ct: ct);
    }
}