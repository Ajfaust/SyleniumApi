using SyleniumApi.Models.Entities;

namespace SyleniumApi.Models.Dtos
{
    public class GetAccountDto
    {
        public int AccountId { get; set; }

        public string? AccountName { get; set; }

        public ICollection<GetTransactionDto> Transactions { get; set; } = new List<GetTransactionDto>();
    }
}