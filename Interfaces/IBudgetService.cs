using AutoBudget_Backend.Models;

public interface IBudgetService
{
    Task<List<Transaction>> ProcessBudgetCsvAsync(IFormFile file);
}