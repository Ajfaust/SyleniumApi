using FastEndpoints;
using FluentValidation;
using SyleniumApi.DbContexts;
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
        Put("/api/financial-accounts/{Id:int}");
        AllowAnonymous();
        DontThrowIfValidationFails();
    }

    public override async Task HandleAsync(UpdateFinancialAccountCommand cmd, CancellationToken ct)
    {
        if (ValidationFailed)
        {
            foreach (var f in ValidationFailures)
                logger.Error("{prop} failed validation: {msg}", f.PropertyName, f.ErrorMessage);

            await SendErrorsAsync(cancellation: ct);
            return;
        }

        var fa = await context.FinancialAccounts.FindAsync(cmd.Id, ct);
        if (fa is null)
        {
            logger.Error("Financial account with id {id} not found", cmd.Id);
            await SendNotFoundAsync(ct);
            return;
        }

        fa.FinancialAccountName = cmd.Name;
        context.FinancialAccounts.Update(fa);
        await context.SaveChangesAsync(ct);

        await SendAsync(new UpdateFinancialAccountResponse(fa.FinancialAccountId, fa.FinancialAccountName),
            cancellation: ct);
    }
}