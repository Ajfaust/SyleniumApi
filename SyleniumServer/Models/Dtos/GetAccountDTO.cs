using SyleniumServer.Models.Entities;

namespace SyleniumServer.Models.Dtos
{
    public class GetAccountDTO
    {
        public int AccountId { get; set; }

        public string Name { get; set; }

        public double Balance { get; set; }
        
        public ICollection<GetTransactionsDTO> Transactions { get; set; }
    }
}