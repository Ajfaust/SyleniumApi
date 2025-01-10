using SyleniumApi.Features.FinancialAccountCategories.Contracts;

namespace SyleniumApi.Features.FinancialAccountCategories;

public static class FaMappingExtensions
{
    public static CreateFaCategory.Request MapCreateFaCategoryRequest(this CreateFaCategoryRequest request)
    {
        return new CreateFaCategory.Request()
        {
            CategoryName = request.CategoryName,
            CategoryType = request.CategoryType
        };
    }
    
    // public static UpdateLedger.Request MapUpdateLedgerRequest(this UpdateLedgerRequest request)
    // {
    //     return new UpdateLedger.Request()
    //     {
    //         LedgerId = request.LedgerId,
    //         LedgerName = request.LedgerName
    //     };
    // }
}