using Carter;
using FluentResults;
using MediatR;
using SyleniumApi.DbContexts;

namespace SyleniumApi.Features.Ledgers;

public class DeleteLedger
{
    public class Request : IRequest<Result>
    {
        public int LegderId { get; set; }
    }
    
    internal sealed class Handler(SyleniumDbContext context) : IRequestHandler<Request, Result>
    {
        public async Task<Result> Handle(Request request, CancellationToken cancellationToken)
        {
            var ledger = await context.Ledgers.FindAsync(request.LegderId, cancellationToken);
            if (ledger == null)
            {
                return Result.Fail("Ledger not found");
            }

            context.Ledgers.Remove(ledger);
            await context.SaveChangesAsync(cancellationToken);
            
            return Result.Ok();
        }
    }
}

public class DeleteLedgerEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("api/ledgers/{id:int}", async (int id, ISender sender) =>
        {
            var query = new DeleteLedger.Request() { LegderId = id };
            var result = await sender.Send(query);

            return result.IsFailed ? Result.Fail(result.Errors) : Result.Ok();
        });
    }
}