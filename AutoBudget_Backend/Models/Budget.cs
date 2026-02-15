namespace AutoBudget_Backend.Models
{
    public class Budget
    {
        public int Id { get; set; }
        
        // Link to the Category
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        public decimal Amount { get; set; }
        
        // Helps track budgets for different months
        public string MonthYear { get; set; } = string.Empty; // Format: "MM-YYYY"
    }
}
