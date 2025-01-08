namespace SyleniumApi.Models.Dtos
{
    public class GetTransactionDto
    {
        public int TransactionId { get; set; }
        
        public int AccountId { get; set; }

        public DateTime Date { get; set; }

        public string? Description { get; set; }

        public decimal Inflow { get; set; }

        public decimal Outflow { get; set; }

        public bool Cleared { get; set; }

        public int? TransactionCategoryId { get; set; }
    }
}