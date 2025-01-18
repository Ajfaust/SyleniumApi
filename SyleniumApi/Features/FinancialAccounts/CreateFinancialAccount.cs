using FastEndpoints;
using FluentValidation;
using SyleniumApi.Data.Entities;
using SyleniumApi.DbContexts;
using SyleniumApi.Features.Shared;
using ILogger = Serilog.ILogger;

namespace SyleniumApi.Features.FinancialAccounts;

public record CreateFinancialAccountCommand(string Name, int LedgerId, int FaCategoryId);

public record CreateFinancialAccountResponse(int Id, string Name, int FaCategoryId);

public class CreateFinancialAccountMapper : Mapper<CreateFinancialAccountCommand, CreateFinancialAccountResponse,
    FinancialAccount>
{
    public override Task<FinancialAccount> ToEntityAsync(CreateFinancialAccountCommand cmd,
        CancellationToken ct = default)
    {
        return Task.FromResult(new FinancialAccount
        {
            LedgerId = cmd.LedgerId,
            FinancialAccountName = cmd.Name,
            FinancialAccountCategoryId = cmd.FaCategoryId
        });
    }

    public override Task<CreateFinancialAccountResponse> FromEntityAsync(FinancialAccount e,
        CancellationToken ct = default)
    {
        return Task.FromResult(new CreateFinancialAccountResponse(e.FinancialAccountId, e.FinancialAccountName,
            e.FinancialAccountCategoryId));
    }
}

public class CreateFinancialAccountValidator : Validator<CreateFinancialAccountCommand>
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
    }

    public override void OnValidationFailed()
    {
        logger.LogValidationErrors(nameof(CreateFinancialAccountEndpoint), ValidationFailures);
    }

    public override async Task HandleAsync(CreateFinancialAccountCommand cmd, CancellationToken ct)
    {
        var fa = await Map.ToEntityAsync(cmd, ct);
        await context.FinancialAccounts.AddAsync(fa, ct);
        await context.SaveChangesAsync(ct);

        await SendMappedAsync(fa, StatusCodes.Status201Created, ct);
    }
}