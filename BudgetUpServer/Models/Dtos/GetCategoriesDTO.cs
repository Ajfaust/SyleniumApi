namespace BudgetUpServer.Models.Dtos
{
    public class GetCategoriesDTO
    {
        public int CategoryId { get; set; }

        public string Name { get; set; }

        public int? ParentId { get; set; }

        public ICollection<GetCategoriesDTO> Subcategories { get; set; }
    }
}