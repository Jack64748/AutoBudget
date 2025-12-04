using Microsoft.AspNetCore.Mvc;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BudgetingAPI.Models; // Adjust to match your actual namespace
using System.Threading.Tasks;

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
                return BadRequest(new { error = "CSV Parsing or DB save failed: " + ex.Message });
            }
        }
    }
}