namespace SyleniumApi.Models.Dtos
{
    public class GetTransactionCategoryDto
    {
        public int CategoryId { get; set; }

        public string? TransactionCategoryName { get; set; }

        public int? ParentCategoryId { get; set; }

        public ICollection<GetTransactionCategoryDto> Subcategories { get; set; } = new List<GetTransactionCategoryDto>();
    }
}