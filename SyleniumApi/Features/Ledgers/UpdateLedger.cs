using Carter;
using FluentResults;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SyleniumApi.DbContexts;

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
            return Result.Fail(validationResult.Errors.Select(e => e.ErrorMessage));
        }
        
        var entity = await context.Ledgers.FindAsync(request.Id);

        if (entity is null)
        {
            return Result.Fail($"Ledger {request.Id} not found");
        }

        entity.LedgerName = request.LedgerName;

        context.Entry(entity).State = EntityState.Modified;
        await context.SaveChangesAsync(cancellationToken);

        var response = new UpdateLedgerResponse(Id: entity.LedgerId, LedgerName: entity.LedgerName);

        return Result.Ok(response);
    }
}

public class UpdateLedgerEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/ledgers/{id:int}", async (int id, UpdateLedgerCommand req, ISender sender) =>
        {
            var result = await sender.Send(req);

            return result.IsFailed ? Result.Fail(result.Errors) : Result.Ok(result.Value);
        });
    }
}