using SyleniumApi.Data.Entities;

namespace SyleniumApi.Features.FinancialAccountCategories.Contracts;

public class CreateFaCategoryRequest
{
    public string CategoryName { get; set; } = string.Empty;
    public FinancialCategoryType CategoryType { get; set; }
}