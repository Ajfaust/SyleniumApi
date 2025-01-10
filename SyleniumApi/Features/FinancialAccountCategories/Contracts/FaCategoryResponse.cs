using SyleniumApi.Data.Entities;

namespace SyleniumApi.Features.FinancialAccountCategories.Contracts;

public class FaCategoryResponse
{
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public FinancialCategoryType FinancialCategoryType { get; set; }
}