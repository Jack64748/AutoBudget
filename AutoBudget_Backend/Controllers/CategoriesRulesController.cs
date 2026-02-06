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
    public class CategoryRulesController : ControllerBase
    {
        private readonly BudgetContext _context;

        public CategoryRulesController(BudgetContext context)
        {
            _context = context;
        }

        // GET: api/CategoryRules
        // Returns rules with the associated Category name attached
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryRule>>> GetCategoryRules()
        {
            return await _context.CategoryRules
                .Include(r => r.Category) // This is the "SQL JOIN" that fetches the category name
                .OrderBy(r => r.Keyword)
                .ToListAsync();
        }

        // POST: api/CategoryRules
        // Creates a new rule (keyword + category link) from the React modal
        [HttpPost]
        public async Task<ActionResult<CategoryRule>> PostCategoryRule(CategoryRule rule)
        {
            if (string.IsNullOrWhiteSpace(rule.Keyword))
            {
                return BadRequest("Keyword is required.");
            }

            // Force keyword to uppercase for consistent matching
            rule.Keyword = rule.Keyword.ToUpper();

            _context.CategoryRules.Add(rule);
            await _context.SaveChangesAsync();

            // Refetch with Category data so the frontend gets the name immediately
            var createdRule = await _context.CategoryRules
                .Include(r => r.Category)
                .FirstOrDefaultAsync(r => r.Id == rule.Id);

            return CreatedAtAction(nameof(GetCategoryRules), new { id = rule.Id }, createdRule);
        }

        // DELETE: api/CategoryRules/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoryRule(int id)
        {
            var rule = await _context.CategoryRules.FindAsync(id);
            if (rule == null)
            {
                return NotFound();
            }

            _context.CategoryRules.Remove(rule);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
