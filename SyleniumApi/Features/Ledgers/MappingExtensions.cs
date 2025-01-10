using SyleniumApi.Contracts;

namespace SyleniumApi.Features.Ledgers;

public static class MappingExtensions
{
    public static CreateLedger.Request MapCreateLedgerRequest(this CreateLedgerRequest request)
    {
        return new CreateLedger.Request()
        {
            LedgerName = request.LedgerName
        };
    }
    
    public static UpdateLedger.Request MapUpdateLedgerRequest(this UpdateLedgerRequest request)
    {
        return new UpdateLedger.Request()
        {
            LedgerId = request.LedgerId,
            LedgerName = request.LedgerName
        };
    }
}