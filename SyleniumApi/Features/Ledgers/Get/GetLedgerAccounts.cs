using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using SyleniumApi.DbContexts;
using SyleniumApi.Features.FinancialAccountCategories;
using SyleniumApi.Features.FinancialAccounts;
using SyleniumApi.Features.Transactions;
using ILogger = Serilog.ILogger;

namespace SyleniumApi.Features.Ledgers.Get;

public record GetLedgerAccountsRequest(int Id);

public record GetLedgerAccountsResponse(List<GetFinancialAccountResponse> Accounts);

public class GetLedgerAccountsEndpoint(SyleniumDbContext context, ILogger logger)
    : Endpoint<GetLedgerAccountsRequest, GetLedgerAccountsResponse>
{
    public override void Configure()
    {
        Get("ledgers/{Id:int}/accounts");
        Description(b => b.Produces(400));
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetLedgerAccountsRequest req, CancellationToken ct)
    {
        var ledgerExists = await context.Ledgers.AnyAsync(l => l.Id == req.Id, ct);
        if (!ledgerExists)
        {
            var message = $"Ledger {req.Id} does not exist";
            logger.Error(message);
            AddError(message);
            await SendErrorsAsync(cancellation: ct);
            return;
        }

        var accounts = context
            .FinancialAccounts
            .Where(fa => fa.LedgerId == req.Id)
            .Include(fa => fa.FinancialAccountCategory)
            .Select(fa => new GetFinancialAccountResponse(
                fa.Id,
                fa.Name,
                new GetFaCategoryResponse(fa.FinancialAccountCategory!.Id, fa.FinancialAccountCategory.Name,
                    fa.FinancialAccountCategory.Type),
                new List<GetTransactionResponse>()))
            .ToList();

        await SendAsync(new GetLedgerAccountsResponse(accounts), cancellation: ct);
    }
}