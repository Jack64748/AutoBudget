using CsvHelper.Configuration.Attributes;


// This is a "Blueprint." It tells the C# code and the PostgreSQL database exactly what a 
// transaction looks like (Type, Amount, Date, etc.).

namespace AutoBudget_Backend.Models
{

    // Each public property represents two things at once 
    // In C#: A variable that holds data in the computer's memory.
    // In PostgreSQL: A specific column in the Transactions table.
    // The ? means optional so if blank will store as null and not crash 
    public class Transaction
    {
        // [....] are attributes and are special instructions for CsvHelper 
        [Ignore] // Tell CsvHelper the CSV doesn't have an ID (Postgres will create it)
        public int Id { get; set; }

        public string Type { get; set; }
        public string Product { get; set; }

        // DateTime allows calendar logic
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
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
