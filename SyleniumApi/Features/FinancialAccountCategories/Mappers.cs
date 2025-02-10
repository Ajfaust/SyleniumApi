using SyleniumApi.Data.Entities;

namespace SyleniumApi.Features.FinancialAccountCategories;

public static class Mappers
{
    public static GetFaCategoryResponse ToGetResponse(this FinancialAccountCategory category)
    {
        return new GetFaCategoryResponse(category.Id, category.Name, category.Type);
    }

    public static CreateFaCategoryResponse ToCreateResponse(this FinancialAccountCategory category)
    {
        return new CreateFaCategoryResponse(category.Id, category.Name, category.Type);
    }

    public static UpdateFaCategoryResponse ToUpdateResponse(this FinancialAccountCategory category)
    {
        return new UpdateFaCategoryResponse(category.Id, category.Name, category.Type);
    }
}