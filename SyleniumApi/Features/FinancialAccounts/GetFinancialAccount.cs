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
            .SingleOrDefaultAsync(fa => fa.Id == req.Id, ct);
        if (fa is null)
        {
            logger.Error("Could not find financial account with id {Id}", req.Id);
            await SendNotFoundAsync(ct);
            return;
        }

        var response = fa.ToGetResponse();

        await SendAsync(response, cancellation: ct);
    }
}