using AutoFixture;
using SyleniumApi.Data.Entities;

namespace SyleniumApi.Tests.FinancialAccountCategories;

public static class FaCategoryTestHelpers
{
    public static FinancialAccountCategory MakeFaCategory(this Fixture fixture, int id = 1,
        FinancialCategoryType type = FinancialCategoryType.Asset)
    {
        return fixture.Build<FinancialAccountCategory>()
            .With(fa => fa.FinancialAccountCategoryId, id)
            .With(fa => fa.FinancialCategoryType, type)
            .Create();
    }
}