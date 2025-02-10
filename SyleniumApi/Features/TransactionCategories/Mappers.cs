using SyleniumApi.Data.Entities;

namespace SyleniumApi.Features.TransactionCategories;

public static class Mappers
{
    public static GetTransactionCategoryResponse ToGetResponse(this TransactionCategory category)
    {
        return new GetTransactionCategoryResponse(
            category.Id,
            category.ParentCategoryId,
            category.Name,
            category.SubCategories.Select(c => c.ToGetResponse()).ToList()
        );
    }

    public static CreateTransactionCategoryResponse ToCreateResponse(this TransactionCategory category)
    {
        return new CreateTransactionCategoryResponse(
            category.Id,
            category.ParentCategoryId,
            category.Name
        );
    }

    public static UpdateTransactionCategoryResponse ToUpdateResponse(this TransactionCategory category)
    {
        return new UpdateTransactionCategoryResponse(
            category.Id,
            category.ParentCategoryId,
            category.Name
        );
    }
}