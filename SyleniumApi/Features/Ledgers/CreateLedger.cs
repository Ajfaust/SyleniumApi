using FastEndpoints;
using FluentValidation;
using SyleniumApi.Data.Entities;
using SyleniumApi.DbContexts;
using ILogger = Serilog.ILogger;

namespace SyleniumApi.Features.Ledgers;

public record CreateLedgerCommand(string Name);

public record CreateLedgerResponse(int Id, string Name);

public class CreateLedgerMapper : Mapper<CreateLedgerCommand, CreateLedgerResponse, Ledger>
{
    public override Task<Ledger> ToEntityAsync(CreateLedgerCommand cmd, CancellationToken ct = default)
    {
        return Task.FromResult(new Ledger
        {
            LedgerName = cmd.Name,
            CreatedDate = DateTime.UtcNow
        });
    }

    public override Task<CreateLedgerResponse> FromEntityAsync(Ledger l, CancellationToken ct)
    {
        return Task.FromResult(new CreateLedgerResponse(l.LedgerId, l.LedgerName));
    }
}

public class CreateLedgerValidator : Validator<CreateLedgerCommand>
{
    public CreateLedgerValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Ledger name is required.")
            .MaximumLength(200)
            .WithMessage("Ledger name cannot exceed 200 characters.");
    }
}

public class CreateLedgerEndpoint(SyleniumDbContext context, ILogger logger)
    : Endpoint<CreateLedgerCommand, CreateLedgerResponse, CreateLedgerMapper>
{
    public override void Configure()
    {
        Post("/api/ledgers");
        AllowAnonymous(); // Think about changing later
        DontThrowIfValidationFails();
    }

    public override async Task HandleAsync(CreateLedgerCommand cmd, CancellationToken ct)
    {
        if (ValidationFailed)
        {
            foreach (var f in ValidationFailures)
                logger.Error("{prop} failed validation: {err}", f.PropertyName, f.ErrorMessage);

            await SendErrorsAsync(cancellation: ct);
        }
        else
        {
            var ledger = await Map.ToEntityAsync(cmd, ct);
            await context.Ledgers.AddAsync(ledger, ct);
            await context.SaveChangesAsync(ct);

            await SendMappedAsync(ledger, StatusCodes.Status201Created, ct);
        }
    }
}