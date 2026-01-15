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

namespace CategoryRulesController.Controllers
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
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryRule>>> GetCategoryRules()
    {
        // We use .Include to "JOIN" the Category table so React can show the Name
        return await _context.CategoryRules
            .Include(r => r.Category) 
            .ToListAsync();
    }

    // POST: api/CategoryRules (For your "Add Rule" button)
    [HttpPost]
    public async Task<ActionResult<CategoryRule>> PostRule(CategoryRule rule)
    {
        _context.CategoryRules.Add(rule);
        await _context.SaveChangesAsync();
        return Ok(rule);
    }
}
}
