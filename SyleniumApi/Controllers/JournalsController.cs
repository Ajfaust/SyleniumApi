using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SyleniumApi.DbContexts;
using SyleniumApi.Interfaces;
using SyleniumApi.Models.Dtos;
using SyleniumApi.Models.Entities;
using SyleniumApi.Services;

namespace SyleniumApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JournalsController(IJournalService journalService) : ControllerBase
    {
        private readonly IJournalService _journalService = journalService;
        
        // GET: api/Journals
        [HttpGet]
        public async Task<ActionResult<IEnumerable<JournalDto>>> GetJournals()
        {
            return await _journalService.GetJournals();
        }

        // GET: api/Journals/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<JournalDto>> GetJournal(int id)
        {
            var result = await _journalService.GetJournal(id);
            if (result == null)
            {
                return NotFound();
            }

            return result;
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

            try
            {
                await _journalService.UpdateJournal(id, journalDto);
            }
            catch (DataException)
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/Journals
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Journal>> PostJournal(JournalDto journalDto)
        {
            var result = await _journalService.CreateJournal(journalDto);

            return CreatedAtAction("GetJournal", new { id = result.JournalId }, result);
        }

        // DELETE: api/Journals/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteJournal(int id)
        {
            await _journalService.DeleteJournal(id);

            return NoContent();
        }
    }
}
