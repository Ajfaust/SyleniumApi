using BudgetUpServer.DbContexts;
using BudgetUpServer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BudgetUpServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class LedgersController : ControllerBase
    {
        private readonly BudgetContext _context;

        public LedgersController(BudgetContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets all available Ledgers
        /// </summary>
        /// <returns>An array of ledgers</returns>
        // GET: api/Ledgers
        [HttpGet]
        public IActionResult GetLedgers()
        {
            return Ok(_context.Ledgers.Select(l => new LedgerDto()
            {
                LedgerId = l.LedgerId,
                Name = l.Name,
            }).ToList());
        }

        // GET: api/Ledgers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LedgerDto>> GetLedger(int id)
        {
            var ledger = await _context.Ledgers.FindAsync(id);

            if (ledger == null)
            {
                return NotFound();
            }

            return new LedgerDto()
            {
                LedgerId = ledger.LedgerId,
                Name = ledger.Name
            };
        }

        // PUT: api/Ledgers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLedger(int id, Ledger ledger)
        {
            if (id != ledger.LedgerId)
            {
                return BadRequest();
            }

            _context.Entry(ledger).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LedgerExists(id))
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

        // POST: api/Ledgers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LedgerDto>> PostLedger(Ledger ledger)
        {
            _context.Ledgers.Add(ledger);
            await _context.SaveChangesAsync();

            var ledgerDto = new LedgerDto()
            {
                LedgerId = ledger.LedgerId,
                Name = ledger.Name
            };

            return CreatedAtAction("GetLedger", new { id = ledger.LedgerId }, ledgerDto);
        }

        // DELETE: api/Ledgers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLedger(int id)
        {
            var ledger = await _context.Ledgers.FindAsync(id);
            if (ledger == null)
            {
                return NotFound();
            }

            _context.Ledgers.Remove(ledger);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LedgerExists(int id)
        {
            return _context.Ledgers.Any(e => e.LedgerId == id);
        }
    }
}