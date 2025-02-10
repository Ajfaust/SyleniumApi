using SyleniumApi.Data.Entities;
using SyleniumApi.Features.FinancialAccountCategories;

namespace SyleniumApi.Features.FinancialAccounts;

public static class Mappers
{
    public static GetFinancialAccountResponse ToGetResponse(this FinancialAccount fa)
    {
        return new GetFinancialAccountResponse(fa.Id, fa.Name, fa.FinancialAccountCategory?.ToGetResponse(), []);
    }

    public static CreateFinancialAccountResponse ToCreateResponse(this FinancialAccount fa)
    {
        return new CreateFinancialAccountResponse(fa.Id, fa.Name, fa.FinancialAccountCategoryId);
    }

    public static UpdateFinancialAccountResponse ToUpdateResponse(this FinancialAccount fa)
    {
        return new UpdateFinancialAccountResponse(fa.Id, fa.Name);
    }
}