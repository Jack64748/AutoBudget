using Microsoft.AspNetCore.Mvc;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoBudget_Backend.Models; // Adjust to match your actual namespace
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BudgetController.Controllers
{
[ApiController]
[Route("api/[controller]")]
public class BudgetController : ControllerBase
{
    private readonly IBudgetService _budgetService;

    // We inject the Service instead of the Context
    public BudgetController(IBudgetService budgetService)
    {
        _budgetService = budgetService;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> Upload([FromForm] IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded");

        try
        {
            // The Controller just delegates the work to the Service
            var result = await _budgetService.ProcessBudgetCsvAsync(file);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}
}