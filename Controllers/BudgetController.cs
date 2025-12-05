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
        private readonly BudgetContext _context;

        public BudgetController(BudgetContext context)
        {
            _context = context;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest(new { error = "No file uploaded" });

            List<Transaction> records;
            try
            {
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Delimiter = ",", // Or "\t" if tab-separated
                    IgnoreBlankLines = true,
                    HeaderValidated = null,
                    MissingFieldFound = null
                };

                using (var reader = new StreamReader(file.OpenReadStream()))
                using (var csv = new CsvReader(reader, config))
                {
                    records = csv.GetRecords<Transaction>().ToList();
                }

                await _context.Transactions.AddRangeAsync(records);
                await _context.SaveChangesAsync();

                var all = await _context.Transactions.ToListAsync();
                return Ok(all);
            }
            catch (Exception ex)
            {
                // Check if the inner exception is present and relevant
                var innerException = ex.InnerException?.Message ?? "No inner exception details available.";
    
                // Log the full exception details (best practice)
                // Console.WriteLine(ex.ToString()); 

                // Return the specific inner message to your console/client (for development only)
                return BadRequest(new { 
                    error = "DB Save Failed.", 
                    details = ex.Message, 
                    inner_db_error = innerException 
    });
}
        }
    }
}