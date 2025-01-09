using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SyleniumApi.DbContexts;
using SyleniumApi.Models.Dtos;
using SyleniumApi.Models.Entities;

namespace SyleniumApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LedgersController(SyleniumContext context) : ControllerBase
    {
        // GET: api/Ledgers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LedgerDto>>> GetLedgers()
        {
            return await context.Ledgers
                .Select(ledger => new LedgerDto
                {
                    LedgerId = ledger.LedgerId,
                    LedgerName = ledger.LedgerName
                })
                .ToListAsync();
        }

        // GET: api/Ledgers/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<LedgerDto>> GetLedger(int id)
        {
            var ledger = await context.Ledgers.FindAsync(id);

            if (ledger == null)
            {
                return NotFound();
            }

            return new LedgerDto()
            {
                LedgerId = ledger.LedgerId,
                LedgerName = ledger.LedgerName,
            };
        }

        // PUT: api/Ledgers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutLedger(int id, [FromBody] LedgerDto ledgerDto)
        {
            if (id != ledgerDto.LedgerId)
            {
                return BadRequest();
            }

            var ledger = await context.Ledgers.FindAsync(id);
            if (ledger == null)
            {
                return NotFound();
            }

            ledger.LedgerName = ledgerDto.LedgerName;

            context.Entry(ledger).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
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
        public async Task<ActionResult<LedgerDto>> PostLedger(int journalId, [FromBody] LedgerDto ledgerDto)
        {
            var journal = await context.Journals.FindAsync(journalId);
            if (journal == null)
            {
                return BadRequest();
            }
            
            var ledger = new Ledger()
            {
                LedgerName = ledgerDto.LedgerName,
                Journal = journal
            };
            context.Ledgers.Add(ledger);
            await context.SaveChangesAsync();

            return CreatedAtAction("GetLedger", new { id = ledger.LedgerId }, ledger.LedgerId);
        }

        // DELETE: api/Ledgers/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteLedger(int id)
        {
            var ledger = await context.Ledgers.FindAsync(id);
            if (ledger == null)
            {
                return NotFound();
            }

            context.Ledgers.Remove(ledger);
            await context.SaveChangesAsync();

            return NoContent();
        }

        private bool LedgerExists(int id)
        {
            return context.Ledgers.Any(e => e.LedgerId == id);
        }
    }
}
