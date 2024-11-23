using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SyleniumServer.DbContexts;
using SyleniumServer.Models.Dtos;
using SyleniumServer.Models.Entities;

namespace SyleniumServer.Controllers
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

        // GET: /Transactions
        /// <summary>
        /// Gets all transactions
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<GetTransactionsDTO>>> GetTransactions()
        {
            return await _context
                .Transactions
                .OrderByDescending(t => t.Date)
                .ThenBy(t => t.TransactionId)
                .Select(t => new GetTransactionsDTO
                {
                    TransactionId = t.TransactionId,
                    Date = t.Date,
                    Notes = t.Notes ?? string.Empty,
                    Inflow = t.Inflow,
                    Outflow = t.Outflow,
                    Cleared = t.Cleared,
                    CategoryId = t.TransactionCategoryId,
                })
                .ToListAsync();
        }

        // GET: /Transactions/5
        /// <summary>
        /// Retrieves a single transaction with the given ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetTransactionsDTO>> GetTransaction(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);

            if (transaction == null)
            {
                return NotFound();
            }

            return new GetTransactionsDTO
            {
                TransactionId = transaction.TransactionId,
                AccountId = transaction.AccountId,
                Date = transaction.Date,
                Notes = transaction.Notes ?? string.Empty,
                Inflow = transaction.Inflow,
                Outflow = transaction.Outflow,
                Cleared = transaction.Cleared,
                CategoryId = transaction.TransactionCategoryId,
            };
        }

        // PUT: /Transactions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Updates the transaction with the given ID
        /// </summary>
        /// <param name="id">ID of the transaction</param>
        /// <param name="transactionDTO">Updated transaction values</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PutTransaction(int id, [FromBody] UpdateTransactionDTO transactionDTO)
        {
            if (id != transactionDTO.TransactionId)
            {
                return BadRequest();
            }

            var transaction = await _context.Transactions.FindAsync(id);
            var category = await _context.TransactionCategories.FindAsync(transactionDTO.CategoryId);
            if (transaction == null || category == null)
            {
                return NotFound();
            }

            transaction.Date = transactionDTO.Date;
            transaction.Notes = transactionDTO.Notes;
            transaction.Inflow = transactionDTO.Inflow;
            transaction.Outflow = transactionDTO.Outflow;
            transaction.Cleared = transactionDTO.Cleared;
            transaction.TransactionCategoryId = transactionDTO.CategoryId;

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
                    return StatusCode(500, "Something went wrong updating this transaction.");
                }
            }

            return NoContent();
        }

        // POST: /Transactions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Creates a new transaction
        /// </summary>
        /// <param name="newTransactionDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<NewTransactionDto>> PostTransaction([FromBody] NewTransactionDto newTransactionDto)
        {
            try
            {
                // Verify account ID
                var account = await _context.Accounts.FindAsync(newTransactionDto.AccountId);
                if (account == null)
                {
                    return BadRequest();
                }
                
                // Generate new transaction based on associated DTO received
                var newTransaction = new Transaction()
                {
                    Date = newTransactionDto.Date,
                    Notes = newTransactionDto.Notes,
                    Inflow = newTransactionDto.Inflow,
                    Outflow = newTransactionDto.Outflow,
                    Cleared = newTransactionDto.Cleared,
                    TransactionCategoryId = newTransactionDto.CategoryId,
                    AccountId = account.AccountId
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

        // DELETE: /Transactions/5
        /// <summary>
        /// Removes the transaction with the given ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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