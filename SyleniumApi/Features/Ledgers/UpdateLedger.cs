using FastEndpoints;
using FluentResults;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SyleniumApi.Data.Entities;
using SyleniumApi.DbContexts;
using ILogger = Serilog.ILogger;

namespace SyleniumApi.Features.Ledgers;

public record UpdateLedgerCommand(int Id, string Name, DateTime Date) : IRequest<Result<UpdateLedgerResponse>>;

public record UpdateLedgerResponse(int Id, string Name);

public class UpdateLedgerMapper : Mapper<UpdateLedgerCommand, UpdateLedgerResponse, Ledger>
{
    public override Ledger ToEntity(UpdateLedgerCommand cmd)
    {
        return new Ledger
        {
            LedgerId = cmd.Id,
            LedgerName = cmd.Name,
            CreatedDate = cmd.Date
        };
    }

    public override UpdateLedgerResponse FromEntity(Ledger l)
    {
        return new UpdateLedgerResponse(l.LedgerId, l.LedgerName);
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
        DontThrowIfValidationFails();
    }

    public override async Task HandleAsync(UpdateLedgerCommand cmd, CancellationToken ct)
    {
        if (ValidationFailed)
        {
            ValidationFailures.ForEach(f =>
                logger.Error("{prop} failed validation: {msg}", f.PropertyName, f.ErrorMessage));
            await SendErrorsAsync();
            return;
        }

        var ledger = await context.Ledgers.FindAsync(cmd.Id);
        if (ledger is null)
        {
            logger.Error("Ledger {id} not found", cmd.Id);
            await SendNotFoundAsync();
            return;
        }

        ledger = Map.ToEntity(cmd);
        context.Entry(ledger).State = EntityState.Modified;
        await context.SaveChangesAsync(ct);

        await SendMappedAsync(ledger);
    }
}