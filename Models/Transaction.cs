


namespace BudgetingAPI.Models
{
    public class Transaction
    {
        
        public int Id { get; set; }
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
    }
