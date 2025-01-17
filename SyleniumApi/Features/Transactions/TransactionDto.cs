namespace SyleniumApi.Features.Transactions;

public class TransactionDto
{
    public int? Id { get; set; }

    public int AccountId { get; set; }

    public int CategoryId { get; set; }

    public int VendorId { get; set; }

    public DateTime Date { get; set; } = DateTime.UtcNow;

    public string Description { get; set; } = string.Empty;

    public decimal Inflow { get; set; }

    public decimal Outflow { get; set; }

    public bool Cleared { get; set; }
}