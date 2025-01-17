using FastEndpoints;
using FluentValidation;
using SyleniumApi.Data.Entities;
using SyleniumApi.DbContexts;
using ILogger = Serilog.ILogger;

namespace SyleniumApi.Features.FinancialAccountCategories;

public record UpdateFaCategoryCommand(int Id, int LedgerId, string Name, FinancialCategoryType Type);

public record UpdateFaCategoryResponse(int Id, string Name, FinancialCategoryType Type);

public class
    UpdateFaCategoryMapper : Mapper<UpdateFaCategoryCommand, UpdateFaCategoryResponse, FinancialAccountCategory>
{
    public override FinancialAccountCategory ToEntity(UpdateFaCategoryCommand cmd)
    {
        return new FinancialAccountCategory
        {
            LedgerId = cmd.LedgerId,
            FinancialAccountCategoryId = cmd.Id,
            FinancialAccountCategoryName = cmd.Name,
            FinancialCategoryType = cmd.Type
        };
    }

    public override UpdateFaCategoryResponse FromEntity(FinancialAccountCategory fac)
    {
        return new UpdateFaCategoryResponse(fac.FinancialAccountCategoryId, fac.FinancialAccountCategoryName,
            fac.FinancialCategoryType);
    }
}

public class UpdateFaCategoryValidator : AbstractValidator<UpdateFaCategoryCommand>
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
        DontThrowIfValidationFails();
    }

    public override async Task HandleAsync(UpdateFaCategoryCommand cmd, CancellationToken ct)
    {
        if (ValidationFailed)
        {
            foreach (var f in ValidationFailures)
            {
                logger.Error("{prop} failed validation: {msg}", f.PropertyName, f.ErrorMessage);
            }

            await SendErrorsAsync();
            return;
        }

        var category = await context.FinancialAccountCategories.FindAsync(cmd.Id);
        if (category == null)
        {
            logger.Error("Financial account category with id {id} not found", cmd.Id);
            await SendNotFoundAsync();
            return;
        }

        // Only change allowed fields
        category.FinancialAccountCategoryName = cmd.Name;
        category.FinancialCategoryType = cmd.Type;
        
        context.FinancialAccountCategories.Update(category);
        await context.SaveChangesAsync(ct);

        await SendMappedAsync(category);
    }
}