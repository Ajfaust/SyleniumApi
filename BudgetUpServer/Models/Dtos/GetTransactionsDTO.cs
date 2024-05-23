namespace AllostaServer.Models.Dtos
{
    public class GetTransactionsDTO
    {
        public int TransactionId { get; set; }

        public int LedgerId { get; set; }

        public DateTime Date { get; set; }

        public string Notes { get; set; }

        public decimal Inflow { get; set; }

        public decimal Outflow { get; set; }

        public bool Cleared { get; set; }

        public int CategoryId { get; set; }
    }
}