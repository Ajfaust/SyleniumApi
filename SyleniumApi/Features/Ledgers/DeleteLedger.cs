using Carter;
using FluentResults;
using MediatR;
using SyleniumApi.DbContexts;

namespace SyleniumApi.Features.Ledgers;

public record DeleteLedgerCommand(int Id) : IRequest<Result>;

public class DeleteLedgerHandler(SyleniumDbContext context) : IRequestHandler<DeleteLedgerCommand, Result>
{
    public async Task<Result> Handle(DeleteLedgerCommand request, CancellationToken cancellationToken)
    {
        var ledger = await context.Ledgers.FindAsync(request.Id);
        if (ledger is null)
        {
            return Result.Fail($"Ledger {request.Id} not found");
        }

        context.Ledgers.Remove(ledger);

        await context.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}

public class DeleteLedgerEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/ledgers/{id:int}", async (int id, ISender sender) =>
        {
            var command = new DeleteLedgerCommand(Id: id);

            var result = await sender.Send(command);

            return result.IsFailed ? Result.Fail(result.Errors) : Result.Ok();
        });
    }
}