using Carter;
using FluentResults;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SyleniumApi.Contracts;
using SyleniumApi.DbContexts;

namespace SyleniumApi.Features.Ledgers;

public static class UpdateLedger
{
    public class Request : IRequest<Result<LedgerResponse>>
    {
        public int LedgerId { get; set; }
        public string LedgerName { get; set; } = string.Empty;
    }

    public class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(r => r.LedgerName)
                .NotEmpty()
                .MaximumLength(200);
        }
    }

    internal sealed class Handler(SyleniumDbContext context, IValidator<Request> validator) : IRequestHandler<Request, Result<LedgerResponse>>
    {
        public async Task<Result<LedgerResponse>> Handle(Request request, CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                return Result.Fail(validationResult.Errors.Select(e => e.ErrorMessage));
            }
            
            var ledger = await context.Ledgers.FindAsync(request.LedgerId, cancellationToken);
            if (ledger == null)
            {
                return Result.Fail<LedgerResponse>($"Ledger with id {request.LedgerId} not found");
            }
            
            ledger.LedgerName = request.LedgerName;
            
            context.Entry(ledger).State = EntityState.Modified;
            await context.SaveChangesAsync(cancellationToken);

            var response = new LedgerResponse()
            {
                LedgerId = ledger.LedgerId,
                LedgerName = ledger.LedgerName,
            };

            return Result.Ok(response);
        }
    }
}

public class UpdateLedgerEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("api/ledgers", async (UpdateLedgerRequest request, ISender sender) =>
        {
            var command = request.MapUpdateLedgerRequest();
            
            var result = await sender.Send(command);
                
            return result.IsFailed ? Result.Fail(result.Errors) : Result.Ok(result);
        });
    }
}