namespace SyleniumServer.Models.Dtos
{
    public class NewCategoryDTO
    {
        public string Name { get; set; }

        public int? ParentId { get; set; }
    }
}