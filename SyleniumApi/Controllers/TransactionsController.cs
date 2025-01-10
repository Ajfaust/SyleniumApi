using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SyleniumApi.DbContexts;
using SyleniumApi.Models.Dtos;
using SyleniumApi.Models.Entities;

namespace SyleniumApi.Controllers
{
    /// <summary>
    /// Controller for Transaction related functions
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class TransactionsController(SyleniumDbContext dbContext, ILogger<TransactionsController> logger)
        : ControllerBase
    {
        // GET: /Transactions
        /// <summary>
        /// Gets all transactions
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<GetTransactionDto>>> GetTransactions()
        {
            return await dbContext
                .Transactions
                .OrderByDescending(t => t.Date)
                .ThenBy(t => t.TransactionId)
                .Select(t => new GetTransactionDto
                {
                    TransactionId = t.TransactionId,
                    Date = t.Date,
                    Description = t.Description ?? string.Empty,
                    Inflow = t.Inflow,
                    Outflow = t.Outflow,
                    Cleared = t.Cleared,
                    TransactionCategoryId = t.TransactionCategoryId,
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
        public async Task<ActionResult<GetTransactionDto>> GetTransaction(int id)
        {
            var transaction = await dbContext.Transactions.FindAsync(id);

            if (transaction == null)
            {
                return NotFound();
            }

            return new GetTransactionDto
            {
                TransactionId = transaction.TransactionId,
                AccountId = transaction.AccountId,
                Date = transaction.Date,
                Description = transaction.Description ?? string.Empty,
                Inflow = transaction.Inflow,
                Outflow = transaction.Outflow,
                Cleared = transaction.Cleared,
                TransactionCategoryId = transaction.TransactionCategoryId,
            };
        }

        // PUT: /Transactions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Updates the transaction with the given ID
        /// </summary>
        /// <param name="id">ID of the transaction</param>
        /// <param name="transactionDto">Updated transaction values</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PutTransaction(int id, [FromBody] UpdateTransactionDto transactionDto)
        {
            if (id != transactionDto.TransactionId)
            {
                return BadRequest();
            }

            var transaction = await dbContext.Transactions.FindAsync(id);
            var category = await dbContext.TransactionCategories.FindAsync(transactionDto.TransactionCategoryId);
            if (transaction == null || category == null)
            {
                return NotFound();
            }

            transaction.Date = transactionDto.Date;
            transaction.Description = transactionDto.Description;
            transaction.Inflow = transactionDto.Inflow;
            transaction.Outflow = transactionDto.Outflow;
            transaction.Cleared = transactionDto.Cleared;
            transaction.TransactionCategoryId = transactionDto.TransactionCategoryId;

            dbContext.Entry(transaction).State = EntityState.Modified;

            try
            {
                await dbContext.SaveChangesAsync();
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
                var account = await dbContext.Accounts.FindAsync(newTransactionDto.AccountId);
                if (account == null)
                {
                    return BadRequest();
                }
                
                // Generate new transaction based on associated DTO received
                var newTransaction = new Transaction()
                {
                    Date = newTransactionDto.Date,
                    Description = newTransactionDto.Description,
                    Inflow = newTransactionDto.Inflow,
                    Outflow = newTransactionDto.Outflow,
                    Cleared = newTransactionDto.Cleared,
                    TransactionCategoryId = newTransactionDto.TransactionCategoryId,
                    Account = account
                };

                logger.LogInformation($"Posting transaction {newTransaction}");

                dbContext.Transactions.Add(newTransaction);
                await dbContext.SaveChangesAsync();

                return CreatedAtAction("GetTransaction", new { id = newTransaction.TransactionId }, newTransaction);
            }
            catch (Exception ex)
            {
                logger.LogError($"Unable to post transaction: {ex.Message}");
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
            var transaction = await dbContext.Transactions.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }

            dbContext.Transactions.Remove(transaction);
            await dbContext.SaveChangesAsync();

            return NoContent();
        }

        private bool TransactionExists(int id)
        {
            return dbContext.Transactions.Any(e => e.TransactionId == id);
        }
    }
}