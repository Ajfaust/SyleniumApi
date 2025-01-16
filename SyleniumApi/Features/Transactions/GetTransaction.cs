using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SyleniumApi.DbContexts;
using SyleniumApi.Shared;

namespace SyleniumApi.Features.Transactions;

public record GetTransactionRequest(int Id) : IRequest<Result<GetTransactionResponse>>;

public record GetTransactionResponse(
    int Id,
    int CategoryId,
    int VendorId,
    DateTime Date,
    string Description,
    decimal Inflow,
    decimal Outflow,
    bool Cleared);

public class GetTransactionHandler(SyleniumDbContext context)
    : IRequestHandler<GetTransactionRequest, Result<GetTransactionResponse>>
{
    public async Task<Result<GetTransactionResponse>> Handle(GetTransactionRequest request,
        CancellationToken cancellationToken)
    {
        var transaction = await context.Transactions.FindAsync(request.Id);
        if (transaction is null)
            return new EntityNotFoundError("Transaction not found");

        var response = new GetTransactionResponse(
            transaction.TransactionId,
            transaction.TransactionCategoryId,
            transaction.VendorId,
            transaction.Date,
            transaction.Description ?? string.Empty,
            transaction.Inflow,
            transaction.Outflow,
            transaction.Cleared
        );

        return Result.Ok(response);
    }
}

public partial class TransactionsController
{
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GetTransactionResponse>> GetTransaction(int id, ISender sender)
    {
        var request = new GetTransactionRequest(id);
        var result = await sender.Send(request);

        return result.HasError<EntityNotFoundError>() ? NotFound(result.Errors) : Ok(result.Value);
    }
}