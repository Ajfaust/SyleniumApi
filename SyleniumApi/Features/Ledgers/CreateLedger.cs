using FluentResults;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyleniumApi.Data.Entities;
using SyleniumApi.DbContexts;
using SyleniumApi.Shared;

namespace SyleniumApi.Features.Ledgers;

public record CreateLedgerCommand(string Name) : IRequest<Result<CreateLedgerResponse>>;

public record CreateLedgerResponse(int Id, string Name);

public class CreateLedgerValidator : AbstractValidator<CreateLedgerCommand>
{
    public CreateLedgerValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
    }
}

public class CreateLedgerHandler(SyleniumDbContext context, IValidator<CreateLedgerCommand> validator)
    : IRequestHandler<CreateLedgerCommand, Result<CreateLedgerResponse>>
{
    public async Task<Result<CreateLedgerResponse>> Handle(CreateLedgerCommand command,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            return new ValidationError(errors: validationResult.Errors);

        var entity = new Ledger
        {
            LedgerName = command.Name,
            CreatedDate = DateTime.UtcNow
        };

        context.Ledgers.Add(entity);

        await context.SaveChangesAsync(cancellationToken);

        var response = new CreateLedgerResponse(entity.LedgerId, entity.LedgerName);
        return Result.Ok(response);
    }
}

public partial class LedgersController
{
    // POST /api/ledgers
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<CreateLedgerResponse>> CreateLedger([FromBody] CreateLedgerCommand command,
        ISender sender)
    {
        try
        {
            var result = await sender.Send(command);

            if (result.HasError<ValidationError>())
            {
                logger.LogValidationError(result);
                return BadRequest(result.Errors);
            }

            logger.Information($"Ledger {result.Value.Id} created successfully");
            return CreatedAtAction("GetLedger", new { id = result.Value.Id }, result.Value);
        }
        catch (Exception ex)
        {
            const string message = "Unexpected error creating new Ledger";
            logger.Error(message, ex);
            return StatusCode(StatusCodes.Status500InternalServerError, message);
        }
    }
}