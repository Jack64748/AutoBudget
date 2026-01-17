using Microsoft.AspNetCore.Mvc;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoBudget_Backend.Models; 
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AutoBudget_Backend.Models;

namespace AutoBudget_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly BudgetContext _context;

        // The constructor injects the Database Context so we can talk to PostgreSQL
        public CategoriesController(BudgetContext context)
        {
            _context = context;
        }

        // GET: api/Categories
        // Returns the list of all categories for the left table in React
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            return await _context.Categories
                .OrderBy(c => c.Id) // Keeps them alphabetical
                .ToListAsync();
        }

        // POST: api/Categories
        // Receives the new category from your React Modal
        [HttpPost]
        public async Task<ActionResult<Category>> PostCategory(Category category)
        {
            if (string.IsNullOrWhiteSpace(category.Name))
            {
                return BadRequest("Category name cannot be empty.");
            }

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            // Returns a 201 Created status and the new Category object (with its generated ID)
            return CreatedAtAction(nameof(GetCategories), new { id = category.Id }, category);
        }

        // PUT: api/Categories/5
        // Handles the "Edit" button logic
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(int id, Category category)
        {
            if (id != category.Id)
            {
                return BadRequest();
            }

            _context.Entry(category).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Categories.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        // DELETE: api/Categories/5
        // Handles the "Delete" button logic
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            // Note: If transactions exist for this category, 
            // Postgres might block this delete due to Foreign Key constraints.
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}