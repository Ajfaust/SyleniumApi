using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SyleniumApi.DbContexts;
using SyleniumApi.Models.Dtos;
using SyleniumApi.Models.Entities;

namespace SyleniumApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JournalsController(SyleniumContext context) : ControllerBase
    {
        // GET: api/Journals
        [HttpGet]
        public async Task<ActionResult<IEnumerable<JournalDto>>> GetJournals()
        {
            return await context
                .Journals
                .Select(j => new JournalDto()
                {
                    JournalId = j.JournalId,
                    JournalName = j.JournalName,
                })
                .ToListAsync();
        }

        // GET: api/Journals/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<JournalDto>> GetJournal(int id)
        {
            var journal = await context.Journals.FindAsync(id);

            if (journal == null)
            {
                return NotFound();
            }

            return new JournalDto()
            {
                JournalId = journal.JournalId,
                JournalName = journal.JournalName,
            };
        }

        // PUT: api/Journals/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutJournal(int id, JournalDto journalDto)
        {
            if (id != journalDto.JournalId)
            {
                return BadRequest();
            }

            var journal = await context.Journals.FindAsync(id);
            
            if (journal == null)
            {
                return NotFound();
            }
            
            journal.JournalId = journalDto.JournalId;
            journal.JournalName = journalDto.JournalName;

            context.Entry(journal).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JournalExists(id))
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

        // POST: api/Journals
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Journal>> PostJournal(JournalDto journalDto)
        {
            var journal = new Journal()
            {
                JournalId = journalDto.JournalId,
                JournalName = journalDto.JournalName,
            };
            
            context.Journals.Add(journal);
            await context.SaveChangesAsync();

            return CreatedAtAction("GetJournal", new { id = journal.JournalId }, journal);
        }

        // DELETE: api/Journals/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteJournal(int id)
        {
            var journal = await context.Journals.FindAsync(id);
            if (journal == null)
            {
                return NotFound();
            }

            context.Journals.Remove(journal);
            await context.SaveChangesAsync();

            return NoContent();
        }

        private bool JournalExists(int id)
        {
            return context.Journals.Any(e => e.JournalId == id);
        }
    }
}
