using FastEndpoints;
using Microsoft.EntityFrameworkCore;
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
    : Endpoint<GetFinancialAccountQuery, GetFinancialAccountResponse, GetTransactionMapper>
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
            .SingleOrDefaultAsync(fa => fa.Id == req.Id, ct);
        if (fa is null)
        {
            logger.Error("Could not find financial account with id {Id}", req.Id);
            await SendNotFoundAsync(ct);
            return;
        }

        var categoryResponse =
            new GetFaCategoryResponse(fa.FinancialAccountCategoryId, fa.FinancialAccountCategory!.Name,
                fa.FinancialAccountCategory!.Type);
        var transactions = fa.Transactions
            .Select(t => new GetTransactionResponse(
                t.Id,
                Date: t.Date,
                AccountId: t.FinancialAccountId,
                CategoryId: t.TransactionCategoryId,
                VendorId: t.VendorId,
                Description: t.Description ?? string.Empty,
                Inflow: t.Inflow,
                Outflow: t.Outflow,
                Cleared: t.Cleared
            ))
            .ToList();

        var response = new GetFinancialAccountResponse(fa.Id, fa.Name, categoryResponse, transactions);
        await SendAsync(response);
    }
}