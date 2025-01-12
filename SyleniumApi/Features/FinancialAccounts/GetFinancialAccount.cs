using Carter;
using FluentResults;
using MediatR;
using SyleniumApi.DbContexts;

namespace SyleniumApi.Features.FinancialAccounts;

public record GetFinancialAccountQuery(int Id) : IRequest<Result<GetFinancialAccountResponse>>;

public record GetFinancialAccountResponse(int Id, string Name);

public class GetFinancialAccountHandler(SyleniumDbContext context)
    : IRequestHandler<GetFinancialAccountQuery, Result<GetFinancialAccountResponse>>
{
    public async Task<Result<GetFinancialAccountResponse>> Handle(GetFinancialAccountQuery request,
        CancellationToken cancellationToken)
    {
        var entity = await context.FinancialAccounts.FindAsync(request.Id);

        if (entity == null)
            return Result.Fail<GetFinancialAccountResponse>("Financial account not found");

        var response = new GetFinancialAccountResponse(entity.FinancialAccountId, entity.FinancialAccountName);

        return Result.Ok(response);
    }
}

public class GetFinancialAccountEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/financial-accounts/{id:int}", async (GetFinancialAccountQuery req, ISender sender) =>
        {
            var id = req.Id;
            var result = await sender.Send(req);

            return result.IsFailed ? Result.Fail(result.Errors) : Result.Ok();
        });
    }
}