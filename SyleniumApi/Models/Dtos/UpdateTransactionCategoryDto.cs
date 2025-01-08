namespace SyleniumApi.Models.Dtos
{
    public class UpdateTransactionCategoryDto
    {
        public int CategoryId { get; set; }

        public required string Name { get; set; }

        public int? ParentId { get; set; }
    }
}