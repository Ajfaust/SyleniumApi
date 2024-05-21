namespace BudgetUpServer.Models.Dtos
{
    public class UpdateCategoryDTO
    {
        public int CategoryId { get; set; }

        public string Name { get; set; }

        public int? ParentId { get; set; }
    }
}