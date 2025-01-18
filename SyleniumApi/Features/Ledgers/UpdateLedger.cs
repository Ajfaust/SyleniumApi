using FastEndpoints;
using FluentValidation;
using SyleniumApi.Data.Entities;
using SyleniumApi.DbContexts;
using SyleniumApi.Features.Shared;
using ILogger = Serilog.ILogger;

namespace SyleniumApi.Features.Ledgers;

public record UpdateLedgerCommand(int Id, string Name);

public record UpdateLedgerResponse(int Id, string Name);

public class UpdateLedgerMapper : Mapper<UpdateLedgerCommand, UpdateLedgerResponse, Ledger>
{
    public override Task<UpdateLedgerResponse> FromEntityAsync(Ledger l, CancellationToken ct = default)
    {
        return Task.FromResult(new UpdateLedgerResponse(l.LedgerId, l.LedgerName));
    }
}

public class UpdateLedgerValidator : Validator<UpdateLedgerCommand>
{
    public UpdateLedgerValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
    }
}

public class UpdateLedgerEndpoint(SyleniumDbContext context, ILogger logger)
    : Endpoint<UpdateLedgerCommand, UpdateLedgerResponse, UpdateLedgerMapper>
{
    public override void Configure()
    {
        Put("/api/ledgers/{Id:int}");
        AllowAnonymous();
    }

    public override void OnValidationFailed()
    {
        logger.LogValidationErrors(nameof(UpdateLedgerEndpoint), ValidationFailures);
    }

    public override async Task HandleAsync(UpdateLedgerCommand cmd, CancellationToken ct)
    {
        if (ValidationFailed)
        {
            ValidationFailures.ForEach(f =>
                logger.Error("{prop} failed validation: {msg}", f.PropertyName, f.ErrorMessage));
            await SendErrorsAsync(cancellation: ct);
            return;
        }

        var ledger = await context.Ledgers.FindAsync(cmd.Id, ct);
        if (ledger is null)
        {
            logger.Error("Ledger {id} not found", cmd.Id);
            await SendNotFoundAsync(ct);
            return;
        }

        ledger.LedgerName = cmd.Name;
        context.Ledgers.Update(ledger);
        await context.SaveChangesAsync(ct);

        await SendMappedAsync(ledger, ct: ct);
    }
}