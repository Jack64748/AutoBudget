using Microsoft.AspNetCore.Mvc;
using AutoBudget_Backend.Models;
using AutoBudget_Backend.Services;

namespace AutoBudget_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TheBudgetController : ControllerBase
    {
        private readonly ITheBudgetService _service;
        public TheBudgetController(ITheBudgetService service) => _service = service;

        [HttpGet("{monthYear}")]
        public async Task<ActionResult<IEnumerable<Budget>>> GetBudgets(string monthYear)
        {
            return Ok(await _service.GetBudgetsByMonthAsync(monthYear));
        }

        [HttpPost]
        public async Task<ActionResult<Budget>> PostBudget(Budget budget)
        {
            var result = await _service.CreateOrUpdateBudgetAsync(budget);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBudget(int id)
        {
            var deleted = await _service.DeleteBudgetAsync(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}