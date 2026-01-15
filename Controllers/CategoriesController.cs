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

namespace CategoriesController.Controllers
{
[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly BudgetContext _context;

    public CategoriesController(BudgetContext context)
    {
        _context = context;
    }

    // GET: api/Categories
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
    {
        return await _context.Categories.OrderBy(c => c.Name).ToListAsync();
    }

    // POST: api/Categories (For your "Add" button)
    [HttpPost]
    public async Task<ActionResult<Category>> PostCategory(Category category)
    {
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
        return Ok(category);
    }
}
}
