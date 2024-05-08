namespace BudgetUpServer.Models.Dtos
{
    public class GetAllTransactionsDTO
    {
        public int TransactionId { get; set; }

        public DateOnly Date { get; set; }

        public string Notes { get; set; }

        public decimal Inflow { get; set; }

        public decimal Outflow { get; set; }

        public bool Cleared { get; set; }
    }
}