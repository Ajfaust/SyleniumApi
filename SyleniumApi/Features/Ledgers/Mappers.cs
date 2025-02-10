using SyleniumApi.Data.Entities;
using SyleniumApi.Features.Ledgers.Get;

namespace SyleniumApi.Features.Ledgers;

public static class Mappers
{
    public static GetLedgerResponse ToGetResponse(this Ledger ledger)
    {
        return new GetLedgerResponse(ledger.Id, ledger.Name, ledger.IsActive);
    }

    public static UpdateLedgerResponse ToUpdateResponse(this Ledger ledger)
    {
        return new UpdateLedgerResponse(ledger.Id, ledger.Name, ledger.IsActive);
    }
}