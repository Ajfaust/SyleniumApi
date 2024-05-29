using AllostaServer.DbContexts;
using AllostaServer.Models.Dtos;
using AllostaServer.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AllostaServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class AccountsController : ControllerBase
    {
        private readonly BudgetContext _context;
        private readonly ILogger<AccountsController> _logger;

        public AccountsController(BudgetContext context, ILogger<AccountsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: /Accounts
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<GetAccountDTO>>> GetAccounts()
        {
            return await _context.Accounts
                .Select(fa => new GetAccountDTO
                {
                    AccountId = fa.AccountId,
                    Name = fa.Name,
                    Balance = fa.Balance,
                })
                .ToListAsync();
        }

        // GET: /Accounts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GetAccountDTO>> GetAccount(int id)
        {
            var Account = await _context
                .Accounts
                .Include(fa => fa.Transactions)
                .FirstOrDefaultAsync(fa => fa.AccountId == id);

            if (Account == null)
            {
                return NotFound();
            }

            return new GetAccountDTO
            {
                AccountId = Account.AccountId,
                Name = Account.Name,
                Balance = Account.Balance
            };
        }

        // PUT: /Accounts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAccount(int id, [FromBody] NewEditAccountDTO AccountDto)
        {
            Account? Account = await _context.Accounts.FindAsync(id);
            if (id != Account?.AccountId)
            {
                return BadRequest();
            }

            Account.Name = AccountDto.Name;
            Account.Balance = AccountDto.Balance;

            _context.Entry(Account).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccountExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: /Accounts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<NewEditAccountDTO>> PostAccount([FromBody] NewEditAccountDTO AccountDto)
        {
            var newAccount = new Account
            {
                Name = AccountDto.Name,
                Balance = AccountDto.Balance,
            };
            _context.Accounts.Add(newAccount);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAccount", new { id = newAccount.AccountId }, AccountDto);
        }

        // DELETE: /Accounts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            var Account = await _context.Accounts.FindAsync(id);
            if (Account == null)
            {
                return NotFound();
            }

            _context.Accounts.Remove(Account);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AccountExists(int id)
        {
            return _context.Accounts.Any(e => e.AccountId == id);
        }
    }
}