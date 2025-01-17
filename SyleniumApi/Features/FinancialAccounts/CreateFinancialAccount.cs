using FastEndpoints;
using FluentValidation;
using SyleniumApi.Data.Entities;
using SyleniumApi.DbContexts;
using ILogger = Serilog.ILogger;

namespace SyleniumApi.Features.FinancialAccounts;

public record CreateFinancialAccountCommand(string Name, int LedgerId, int FaCategoryId);

public record CreateFinancialAccountResponse(int Id, string Name, int FaCategoryId);

public class CreateFinancialAccountMapper : Mapper<CreateFinancialAccountCommand, CreateFinancialAccountResponse,
    FinancialAccount>
{
    public override FinancialAccount ToEntity(CreateFinancialAccountCommand cmd)
    {
        return new FinancialAccount
        {
            LedgerId = cmd.LedgerId,
            FinancialAccountName = cmd.Name,
            FinancialAccountCategoryId = cmd.FaCategoryId
        };
    }

    public override CreateFinancialAccountResponse FromEntity(FinancialAccount e)
    {
        return new CreateFinancialAccountResponse(e.FinancialAccountId, e.FinancialAccountName,
            e.FinancialAccountCategoryId);
    }
}

public class CreateFinancialAccountValidator : AbstractValidator<CreateFinancialAccountCommand>
{
    public CreateFinancialAccountValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.LedgerId).NotEmpty();
        RuleFor(x => x.FaCategoryId).NotEmpty();
    }
}

public class CreateFinancialAccountEndpoint(SyleniumDbContext context, ILogger logger)
    : Endpoint<CreateFinancialAccountCommand, CreateFinancialAccountResponse, CreateFinancialAccountMapper>
{
    public override void Configure()
    {
        Post("/api/financial-accounts");
        AllowAnonymous();
        DontThrowIfValidationFails();
    }

    public override async Task HandleAsync(CreateFinancialAccountCommand cmd, CancellationToken ct)
    {
        if (ValidationFailed)
        {
            foreach (var f in ValidationFailures)
                logger.Error("{prop} has failed validation: {msg}", f.PropertyName, f.ErrorMessage);

            await SendErrorsAsync();
            return;
        }

        var fa = Map.ToEntity(cmd);
        await context.FinancialAccounts.AddAsync(fa, ct);
        await context.SaveChangesAsync(ct);

        await SendMappedAsync(fa, StatusCodes.Status201Created);
    }
}