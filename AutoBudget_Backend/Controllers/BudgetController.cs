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


    // Receives the file from React and hands it to the Service.
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

    // Listens for the "Delete All" command.
    [HttpDelete("clear")]
    public async Task<IActionResult> ClearData()
    {
        try
        {
            await _budgetService.ClearAllTransactionsAsync();
            return Ok(new { message = "All data cleared successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }


    // Fetches all transactions to show in the table.
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        // Use the service instead of _context
        var transactions = await _budgetService.GetAllTransactionsAsync();
        return Ok(transactions);
    }


  [HttpPost("reassign")]
    public async Task<IActionResult> ReassignTransactions([FromBody] ReassignRequest request)
    {
        if (string.IsNullOrEmpty(request.Description))
          return BadRequest("Description is required");

        // We call the service instead of using _context directly
        await _budgetService.ReassignTransactionsAsync(request.Description, request.NewCategoryId);
    
        return Ok();
    }
    

}
    public class ReassignRequest
    {
        public string Description { get; set; }
        public int NewCategoryId { get; set; }
    }
}