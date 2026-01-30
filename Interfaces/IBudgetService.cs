using AutoBudget_Backend.Models;

public interface IBudgetService
{
    Task<List<Transaction>> ProcessBudgetCsvAsync(IFormFile file);

    Task ClearAllTransactionsAsync();

    Task<List<Transaction>> GetAllTransactionsAsync();
    
    Task ReassignTransactionsAsync(string description, int newCategoryId);
}