using BudgetUpServer.DbContexts;
using BudgetUpServer.Models.Dtos;
using BudgetUpServer.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BudgetUpServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class TransactionsController : ControllerBase
    {
        private readonly BudgetContext _context;
        private readonly ILogger<TransactionsController> _logger;

        public TransactionsController(BudgetContext context, ILogger<TransactionsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Transactions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetAllTransactionsDTO>>> GetTransactions()
        {
            return await _context
                .Transactions
                .OrderByDescending(t => t.Date)
                .ThenBy(t => t.TransactionId)
                .Select(t => new GetAllTransactionsDTO
                {
                    TransactionId = t.TransactionId,
                    Date = t.Date,
                    Notes = t.Notes,
                    Inflow = t.Inflow,
                    Outflow = t.Outflow,
                    Cleared = t.Cleared,
                })
                .ToListAsync();
        }

        // GET: api/Transactions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Transaction>> GetTransaction(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);

            if (transaction == null)
            {
                return NotFound();
            }

            return transaction;
        }

        // PUT: api/Transactions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTransaction(int id, Transaction transaction)
        {
            if (id != transaction.TransactionId)
            {
                return BadRequest();
            }

            _context.Entry(transaction).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransactionExists(id))
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

        // POST: api/Transactions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Creates a new transaction
        /// </summary>
        /// <param name="newTransactionDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<NewTransactionDto>> PostTransaction([FromBody] NewTransactionDto newTransactionDto)
        {
            try
            {
                // Generate new transaction based on associated DTO received
                var newTransaction = new Transaction()
                {
                    LedgerId = newTransactionDto.LedgerId,
                    Date = newTransactionDto.Date,
                    Notes = newTransactionDto.Notes,
                    Inflow = newTransactionDto.Inflow,
                    Outflow = newTransactionDto.Outflow,
                    Cleared = newTransactionDto.Cleared
                };

                _logger.LogInformation($"Posting transaction {newTransaction}");

                _context.Transactions.Add(newTransaction);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetTransaction", new { id = newTransaction.TransactionId }, newTransaction);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unable to post transaction: {ex.Message}");
                return StatusCode(500, "Something went wrong creating this transaction.");
            }
        }

        // DELETE: api/Transactions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }

            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TransactionExists(int id)
        {
            return _context.Transactions.Any(e => e.TransactionId == id);
        }
    }
}