using CsvHelper.Configuration.Attributes;


namespace AutoBudget_Backend.Models
{
    public class Transaction
    {
        
        [Ignore] // Tell CsvHelper the CSV doesn't have an ID (Postgres will create it)
        public int Id { get; set; }

        public string Type { get; set; }
        public string Product { get; set; }

        [Name("Started Date")] // Map C# 'StartedDate' to CSV 'Started Date'
        public DateTime? StartedDate { get; set; }

        [Name("Completed Date")] // Map C# 'CompletedDate' to CSV 'Completed Date'
        public DateTime? CompletedDate { get; set; }

        public string Description { get; set; }
        public decimal? Amount { get; set; }
        public decimal? Fee { get; set; }
        public string Currency { get; set; }
        public string State { get; set; }
        public decimal? Balance { get; set; }
        }
    }
