using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyleniumApi.DbContexts;
using SyleniumApi.Shared;

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
            return new EntityNotFoundError("Financial account not found");

        var response = new GetFinancialAccountResponse(entity.FinancialAccountId, entity.FinancialAccountName);

        return Result.Ok(response);
    }
}

public partial class FinancialAccountsController
{
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetFinancialAccount(int id, ISender sender)
    {
        var query = new GetFinancialAccountQuery(id);
        var result = await sender.Send(query);

        return result.HasError<EntityNotFoundError>() ? NotFound(result.Errors) : Ok(result.Value);
    }
}