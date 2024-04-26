using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BudgetUpServer.Contexts;
using BudgetUpServer.Entity;

namespace BudgetUpServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpreadsheetsController : ControllerBase
    {
        private readonly BudgetContext _context;

        public SpreadsheetsController(BudgetContext context)
        {
            _context = context;
        }

        // GET: api/Spreadsheets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Spreadsheet>>> GetSpreadsheets()
        {
            return await _context.Spreadsheets.ToListAsync();
        }

        // GET: api/Spreadsheets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Spreadsheet>> GetSpreadsheet(int id)
        {
            var spreadsheet = await _context.Spreadsheets.FindAsync(id);

            if (spreadsheet == null)
            {
                return NotFound();
            }

            return spreadsheet;
        }

        // PUT: api/Spreadsheets/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSpreadsheet(int id, Spreadsheet spreadsheet)
        {
            if (id != spreadsheet.SpreadsheetId)
            {
                return BadRequest();
            }

            _context.Entry(spreadsheet).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SpreadsheetExists(id))
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

        // POST: api/Spreadsheets
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Spreadsheet>> PostSpreadsheet(Spreadsheet spreadsheet)
        {
            _context.Spreadsheets.Add(spreadsheet);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSpreadsheet", new { id = spreadsheet.SpreadsheetId }, spreadsheet);
        }

        // DELETE: api/Spreadsheets/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSpreadsheet(int id)
        {
            var spreadsheet = await _context.Spreadsheets.FindAsync(id);
            if (spreadsheet == null)
            {
                return NotFound();
            }

            _context.Spreadsheets.Remove(spreadsheet);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SpreadsheetExists(int id)
        {
            return _context.Spreadsheets.Any(e => e.SpreadsheetId == id);
        }
    }
}
