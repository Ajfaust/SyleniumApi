using Carter;
using FluentResults;
using MediatR;
using SyleniumApi.DbContexts;

namespace SyleniumApi.Features.FinancialAccounts;

public record DeleteFinancialAccountCommand(int Id) : IRequest<Result>;

public class DeleteFinancialAccountHandler(SyleniumDbContext context)
    : IRequestHandler<DeleteFinancialAccountCommand, Result>
{
    public async Task<Result> Handle(DeleteFinancialAccountCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.FinancialAccounts.FindAsync(request.Id);
        if (entity is null)
            return Result.Fail("Financial account not found");

        context.FinancialAccounts.Remove(entity);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}

public class DeleteFinancialAccountEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/financial-accounts/{id:int}", async (DeleteFinancialAccountCommand req, ISender sender) =>
        {
            var result = await sender.Send(req);

            return result.IsFailed ? Results.BadRequest(result.Errors) : Results.Ok();
        });
    }
}