using Microsoft.AspNetCore.Mvc;
using AutoBudget_Backend.Models;
using AutoBudget_Backend.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoBudget_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryRulesController : ControllerBase
    {
        private readonly IRuleService _ruleService;

        public CategoryRulesController(IRuleService ruleService)
        {
            _ruleService = ruleService;
        }

        // GET: api/CategoryRules
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryRule>>> GetCategoryRules()
        {
            var rules = await _ruleService.GetAllRulesAsync();
            return Ok(rules);
        }

        // POST: api/CategoryRules
        [HttpPost]
        public async Task<ActionResult<CategoryRule>> PostCategoryRule(CategoryRule rule)
        {
            if (string.IsNullOrWhiteSpace(rule.Keyword))
            {
                return BadRequest("Keyword is required.");
            }

            var createdRule = await _ruleService.CreateRuleAsync(rule);
            return CreatedAtAction(nameof(GetCategoryRules), new { id = createdRule.Id }, createdRule);
        }

        // DELETE: api/CategoryRules/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoryRule(int id)
        {
            var success = await _ruleService.DeleteRuleAsync(id);
            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
