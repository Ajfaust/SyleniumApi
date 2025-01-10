using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SyleniumApi.DbContexts;
using SyleniumApi.Models.Dtos;
using SyleniumApi.Models.Entities;

namespace SyleniumApi.Controllers
{
    /// <summary>
    /// Controller for Category related functions
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class CategoriesController(SyleniumDbContext dbContext, ILogger<CategoriesController> logger)
        : ControllerBase
    {
        // GET: /Categories
        /// <summary>
        /// Gets all categories and associated subcategories
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<GetTransactionCategoryDto>>> GetTransactionCategories()
        {
            return await dbContext
                .TransactionCategories
                .Include(c => c.SubCategories)
                .Where(c => c.ParentCategory == null)   // Exclude any subcategories as they are included in the line above
                .Select(c => new GetTransactionCategoryDto
                {
                    CategoryId = c.TransactionCategoryId,
                    TransactionCategoryName = c.TransactionCategoryName,
                    Subcategories = c.SubCategories.Select(sc => new GetTransactionCategoryDto
                    {
                        CategoryId = sc.TransactionCategoryId,
                        TransactionCategoryName = sc.TransactionCategoryName,
                        ParentCategoryId = sc.ParentCategoryId,
                    })
                    .ToList()
                })
                .ToListAsync();
        }

        // GET: /Categories/5
        /// <summary>
        /// Gets a category by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<GetTransactionCategoryDto>> GetTransactionCategory(int id)
        {
            var transactionCategory = await dbContext.TransactionCategories.Include(c => c.SubCategories).FirstOrDefaultAsync(c => c.TransactionCategoryId == id);

            if (transactionCategory == null)
            {
                return NotFound();
            }

            var result = new GetTransactionCategoryDto
            {
                CategoryId = transactionCategory.TransactionCategoryId,
                TransactionCategoryName = transactionCategory.TransactionCategoryName,
                ParentCategoryId = transactionCategory.ParentCategoryId,
                Subcategories = transactionCategory.SubCategories.Select(sc => new GetTransactionCategoryDto { CategoryId = sc.TransactionCategoryId, TransactionCategoryName = sc.TransactionCategoryName, ParentCategoryId = sc.ParentCategoryId }).ToList(),
            };

            return result;
        }

        // PUT: /Categories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Updates a Category
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updatedTransactionCategory"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTransactionCategory(int id, [FromBody] UpdateTransactionCategoryDto updatedTransactionCategory)
        {
            if (id != updatedTransactionCategory.CategoryId)
            {
                return BadRequest();
            }

            var category = await dbContext.TransactionCategories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            category.TransactionCategoryName = updatedTransactionCategory.Name;
            category.ParentCategoryId = updatedTransactionCategory.ParentId;

            dbContext.Entry(category).State = EntityState.Modified;

            try
            {
                await dbContext.SaveChangesAsync();
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

        // POST: /Categories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Creates a new Category
        /// </summary>
        /// <param name="transactionCategory"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<NewTransactionCategoryDto>> PostTransactionCategory([FromBody] NewTransactionCategoryDto transactionCategory)
        {
            try
            {
                var newCategory = new TransactionCategory
                {
                    TransactionCategoryName = transactionCategory.Name,
                    ParentCategoryId = transactionCategory.ParentId,
                    Transactions = []
                };

                dbContext.TransactionCategories.Add(newCategory);
                await dbContext.SaveChangesAsync();

                return CreatedAtAction("GetTransactionCategory", new { id = newCategory.TransactionCategoryId, name = newCategory.TransactionCategoryName }, transactionCategory);
            }
            catch (Exception ex)
            {
                logger.LogError($"Error creating category: {ex.Message}");
                return StatusCode(500, "Unable to create category");
            }
        }

        // DELETE: /Categories/5
        /// <summary>
        /// Deletes a category by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransactionCategory(int id)
        {
            var transactionCategory = await dbContext.TransactionCategories.FindAsync(id);
            if (transactionCategory == null)
            {
                return NotFound();
            }

            dbContext.TransactionCategories.Remove(transactionCategory);
            await dbContext.SaveChangesAsync();

            return NoContent();
        }

        private bool TransactionCategoryExists(int id)
        {
            return dbContext.TransactionCategories.Any(e => e.TransactionCategoryId == id);
        }
    }
}