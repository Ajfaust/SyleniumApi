using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using SyleniumApi.Data.Entities;
using SyleniumApi.DbContexts;
using SyleniumApi.Features.FinancialAccountCategories;
using SyleniumApi.Features.Transactions;
using ILogger = Serilog.ILogger;

namespace SyleniumApi.Features.FinancialAccounts;

public record GetFinancialAccountQuery(int Id);

public record GetFinancialAccountResponse(
    int Id,
    string Name,
    GetFaCategoryResponse? FinancialCategory,
    List<GetTransactionResponse>? Transactions);

public class GetFinancialAccountEndpoint(SyleniumDbContext context, ILogger logger)
    : Endpoint<GetFinancialAccountQuery, GetFinancialAccountResponse>
{
    public override void Configure()
    {
        Get("financial-accounts/{Id:int}");
        Description(b => b.Produces(404));
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetFinancialAccountQuery req, CancellationToken ct)
    {
        var fa = await context
            .FinancialAccounts
            .Include(fa => fa.FinancialAccountCategory)
            .Include(fa => fa.Transactions)
            .ThenInclude(t => t.TransactionCategory)
            .Include(fa => fa.Transactions)
            .ThenInclude(t => t.Vendor)
            .SingleOrDefaultAsync(fa => fa.Id == req.Id, ct);
        if (fa is null)
        {
            logger.Error("Could not find financial account with id {Id}", req.Id);
            await SendNotFoundAsync(ct);
            return;
        }

        try
        {
            await SendAsync(fa.ToGetResponse(), cancellation: ct);
        }
        catch (NullReferenceException ex)
        {
            logger.Error(ex.Message);
            AddError("Unexpected error creating response object.");
            await SendErrorsAsync(StatusCodes.Status500InternalServerError, ct);
        }
    }
}

public static class AccountMappers
{
    public static GetFinancialAccountResponse ToGetResponse(this FinancialAccount account)
    {
        if (account.FinancialAccountCategory == null)
            throw new NullReferenceException("Financial Account Category cannot be null");

        return new GetFinancialAccountResponse(account.Id, account.Name,
            account.FinancialAccountCategory.ToGetResponse(),
            account.Transactions.Select(t => t.ToGetResponse()).ToList());
    }
}