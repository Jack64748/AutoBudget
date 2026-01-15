namespace AutoBudget_Backend.Models
{
    public class CategoryRule
    {
        public int Id { get; set; }
        public string Keyword { get; set; } // e.g., "TESCO", "PUREGYM"
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}