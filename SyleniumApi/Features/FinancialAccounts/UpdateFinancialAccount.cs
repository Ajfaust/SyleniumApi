using FastEndpoints;
using FluentValidation;
using SyleniumApi.DbContexts;
using SyleniumApi.Features.Shared;
using ILogger = Serilog.ILogger;

namespace SyleniumApi.Features.FinancialAccounts;

public record UpdateFinancialAccountCommand(int Id, int LedgerId, string Name);

public record UpdateFinancialAccountResponse(int Id, string Name);

public class UpdateFinancialAccountValidator : Validator<UpdateFinancialAccountCommand>
{
    public UpdateFinancialAccountValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
    }
}

public class UpdateFinancialAccountEndpoint(SyleniumDbContext context, ILogger logger)
    : Endpoint<UpdateFinancialAccountCommand, UpdateFinancialAccountResponse>
{
    public override void Configure()
    {
        Put("financial-accounts/{Id:int}");
        Description(b => b.Produces(404));
        AllowAnonymous();
    }

    public override void OnValidationFailed()
    {
        logger.LogValidationErrors(nameof(UpdateFinancialAccountEndpoint), ValidationFailures);
    }

    public override async Task HandleAsync(UpdateFinancialAccountCommand cmd, CancellationToken ct)
    {
        var fa = await context.FinancialAccounts.FindAsync(cmd.Id, ct);
        if (fa is null)
        {
            logger.Error("Financial account with id {id} not found", cmd.Id);
            await SendNotFoundAsync(ct);
            return;
        }

        fa.Name = cmd.Name;
        context.FinancialAccounts.Update(fa);
        await context.SaveChangesAsync(ct);

        await SendAsync(new UpdateFinancialAccountResponse(fa.Id, fa.Name),
            cancellation: ct);
    }
}