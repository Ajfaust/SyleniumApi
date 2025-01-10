namespace SyleniumApi.Contracts;

public class UpdateLedgerRequest
{
    public int LedgerId { get; set; }
    public string LedgerName { get; set; } = string.Empty;
}