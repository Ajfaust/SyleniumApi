using Carter;
using FluentResults;
using FluentValidation;
using MediatR;
using SyleniumApi.Contracts;
using SyleniumApi.DbContexts;
using SyleniumApi.Models.Entities;

namespace SyleniumApi.Features.Ledgers;

public static class CreateLedger
{
    public class Request : IRequest<Result<LedgerResponse>>
    {
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
            
            var ledger = new Ledger
            {
                LedgerName = request.LedgerName,
                CreatedDate = DateTime.UtcNow,
            };

            context.Ledgers.Add(ledger);
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

public class CreateLedgerEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/ledgers", async (CreateLedgerRequest request, ISender sender) =>
        {
            var command = request.MapCreateLedgerRequest();
            
            var result = await sender.Send(command);
                
            return result.IsFailed ? Result.Fail(result.Errors) : Result.Ok(result);
        });
    }
}