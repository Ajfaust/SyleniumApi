using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SyleniumApi.DbContexts;
using SyleniumApi.Models.Dtos;
using SyleniumApi.Models.Entities;

namespace SyleniumApi.Controllers
{
    /// <summary>
    /// Controller for account related functions
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class AccountsController(SyleniumContext context, ILogger<AccountsController> logger) : ControllerBase
    {
        private readonly ILogger<AccountsController> _logger = logger;

        // GET: /Accounts
        /// <summary>
        /// Gets all accounts
        /// </summary>
        /// <returns> </returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<GetAccountDto>>> GetAccounts()
        {
            return await context.Accounts
                .Select(fa => new GetAccountDto
                {
                    AccountId = fa.AccountId,
                    AccountName = fa.AccountName,
                })
                .ToListAsync();
        }

        // GET: /Accounts/5
        /// <summary>
        /// Gets an Account by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<GetAccountDto>> GetAccount(int id)
        {
            var account = await context
                .Accounts
                .Include(fa => fa.Transactions)
                .FirstOrDefaultAsync(fa => fa.AccountId == id);

            if (account == null)
            {
                return NotFound();
            }

            var accountTransactions = account.Transactions.Select(t => new GetTransactionDto
            {
                TransactionId = t.TransactionId,
                AccountId = t.AccountId,
                TransactionCategoryId = t.TransactionCategoryId,
                Cleared = t.Cleared,
                Date = t.Date,
                Inflow = t.Inflow,
                Outflow = t.Outflow,
                Description = t.Description ?? string.Empty,
            }).ToList();

            return new GetAccountDto
            {
                AccountId = account.AccountId,
                AccountName = account.AccountName,
                Transactions = accountTransactions
            };
        }

        // PUT: /Accounts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Modifies an Account
        /// </summary>
        /// <param name="id"></param>
        /// <param name="accountDto"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAccount(int id, [FromBody] NewEditAccountDTO accountDto)
        {
            Account? account = await context.Accounts.FindAsync(id);
            if (id != account?.AccountId)
            {
                return BadRequest();
            }

            account.AccountName = accountDto.Name;

            context.Entry(account).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
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
        /// <summary>
        /// Creates a new Account
        /// </summary>
        /// <param name="accountDto"></param>
        [HttpPost]
        public async Task<ActionResult<NewEditAccountDTO>> PostAccount([FromBody] NewEditAccountDTO accountDto)
        {
            var newAccount = new Account
            {
                AccountName = accountDto.Name,
            };
            context.Accounts.Add(newAccount);
            await context.SaveChangesAsync();

            return CreatedAtAction("GetAccount", new { id = newAccount.AccountId }, accountDto);
        }

        // DELETE: /Accounts/5
        /// <summary>
        /// Deletes an Account by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            var account = await context.Accounts.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }

            context.Accounts.Remove(account);
            await context.SaveChangesAsync();

            return NoContent();
        }

        private bool AccountExists(int id)
        {
            return context.Accounts.Any(e => e.AccountId == id);
        }
    }
}