namespace SyleniumApi.Models.Dtos
{
    public class NewTransactionCategoryDto
    {
        public required string Name { get; set; }

        public int? ParentId { get; set; }
    }
}