using Microsoft.AspNetCore.Mvc;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BudgetController.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BudgetController : ControllerBase
    {
        public class Transaction
        {
            public string Type { get; set; }
            public string Product { get; set; }
            public string StartedDate { get; set; }
            public string CompletedDate { get; set; }
            public string Description { get; set; }
            public decimal Amount { get; set; }
            public decimal Fee { get; set; }
            public string Currency { get; set; }
            public string State { get; set; }
            public decimal Balance { get; set; }
        }

        [HttpPost("upload")]
        public IActionResult Upload([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest(new { error = "No file uploaded" });

            List<Transaction> records = new List<Transaction>();
            try
            {
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Delimiter = ",", // Your file is tab-separated!
                    IgnoreBlankLines = true,
                    HeaderValidated = null,
                    MissingFieldFound = null
                };

                using (var reader = new StreamReader(file.OpenReadStream()))
                using (var csv = new CsvReader(reader, config))
                {
                    records = csv.GetRecords<Transaction>().ToList();
                }

                // Just return the parsed records!
                return Ok(records);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { error = "CSV Parsing failed: " + ex.Message });
            }
        }
    }
}