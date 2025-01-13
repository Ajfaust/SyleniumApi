using Carter;
using FluentResults;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SyleniumApi.DbContexts;
using SyleniumApi.Shared;

namespace SyleniumApi.Features.Ledgers;

public record UpdateLedgerCommand(int Id, string LedgerName) : IRequest<Result<UpdateLedgerResponse>>;

public record UpdateLedgerResponse(int Id, string LedgerName);

public class UpdateLedgerValidator : AbstractValidator<UpdateLedgerCommand>
{
    public UpdateLedgerValidator()
    {
        RuleFor(x => x.LedgerName).NotEmpty().MaximumLength(200);
    }
}

public class UpdateLedgerHandler(SyleniumDbContext context, IValidator<UpdateLedgerCommand> validator) : IRequestHandler<UpdateLedgerCommand, Result<UpdateLedgerResponse>>
{
    public async Task<Result<UpdateLedgerResponse>> Handle(UpdateLedgerCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            return new ValidationError("One or more properties are invalid");
        }
        
        var entity = await context.Ledgers.FindAsync(request.Id);

        if (entity is null)
        {
            return new EntityNotFoundError($"Ledger {request.Id} not found");
        }

        entity.LedgerName = request.LedgerName;

        context.Entry(entity).State = EntityState.Modified;
        await context.SaveChangesAsync(cancellationToken);

        var response = new UpdateLedgerResponse(Id: entity.LedgerId, LedgerName: entity.LedgerName);

        return Result.Ok(response);
    }
}

public partial class LedgersController
{
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateLedger(int id, UpdateLedgerCommand command, ISender sender)
    {
        if (id != command.Id) {
            return BadRequest("Id in the URL does not match the Id in the request body");
        }
        
        var result = await sender.Send(command);

        if (result.HasError<ValidationError>())
        {
            return BadRequest(result.Errors);
        }

        return result.HasError<EntityNotFoundError>() ? NotFound(result.Errors) : Ok(result.Value);
    }
}