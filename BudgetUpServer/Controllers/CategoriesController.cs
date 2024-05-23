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
    public class CategoriesController : ControllerBase
    {
        private readonly BudgetContext _context;
        private readonly ILogger<CategoriesController> _logger;

        public CategoriesController(BudgetContext context, ILogger<CategoriesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/Categories
        /// <summary>
        /// Gets all categories and associated subcategories
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<GetCategoriesDTO>>> GetTransactionCategories()
        {
            return await _context
                .TransactionCategories
                .Include(c => c.SubCategories)
                .Where(c => c.ParentCategory == null)   // Exclude any subcategories as they are included in the line above
                .Select(c => new GetCategoriesDTO
                {
                    CategoryId = c.TransactionCategoryId,
                    Name = c.Name,
                    Subcategories = c.SubCategories.Select(sc => new GetCategoriesDTO
                    {
                        CategoryId = sc.TransactionCategoryId,
                        Name = sc.Name,
                        ParentId = sc.ParentCategoryId,
                    })
                    .ToList()
                })
                .ToListAsync();
        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GetCategoriesDTO>> GetTransactionCategory(int id)
        {
            var transactionCategory = await _context.TransactionCategories.Include(c => c.SubCategories).FirstOrDefaultAsync(c => c.TransactionCategoryId == id);

            if (transactionCategory == null)
            {
                return NotFound();
            }

            var result = new GetCategoriesDTO
            {
                CategoryId = transactionCategory.TransactionCategoryId,
                Name = transactionCategory.Name,
                ParentId = transactionCategory.ParentCategoryId,
                Subcategories = transactionCategory.SubCategories.Select(sc => new GetCategoriesDTO { CategoryId = sc.TransactionCategoryId, Name = sc.Name, ParentId = sc.ParentCategoryId }).ToList(),
            };

            return result;
        }

        // PUT: api/Categories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTransactionCategory(int id, [FromBody] UpdateCategoryDTO updatedCategory)
        {
            if (id != updatedCategory.CategoryId)
            {
                return BadRequest();
            }

            var category = await _context.TransactionCategories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            category.Name = updatedCategory.Name;
            category.ParentCategoryId = updatedCategory.ParentId;

            _context.Entry(category).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransactionCategoryExists(id))
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

        // POST: api/Categories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<NewCategoryDTO>> PostTransactionCategory([FromBody] NewCategoryDTO transactionCategory)
        {
            try
            {
                var newCategory = new TransactionCategory
                {
                    Name = transactionCategory.Name,
                    ParentCategoryId = transactionCategory.ParentId,
                };

                _context.TransactionCategories.Add(newCategory);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetTransactionCategory", new { id = newCategory.TransactionCategoryId, name = newCategory.Name }, transactionCategory);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating category: {ex.Message}");
                return StatusCode(500, "Unable to create category");
            }
        }

        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransactionCategory(int id)
        {
            var transactionCategory = await _context.TransactionCategories.FindAsync(id);
            if (transactionCategory == null)
            {
                return NotFound();
            }

            _context.TransactionCategories.Remove(transactionCategory);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TransactionCategoryExists(int id)
        {
            return _context.TransactionCategories.Any(e => e.TransactionCategoryId == id);
        }
    }
}