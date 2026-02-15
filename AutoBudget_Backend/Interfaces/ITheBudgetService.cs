using AutoBudget_Backend.Models;

namespace AutoBudget_Backend.Services
{
    public interface ITheBudgetService
    {
        Task<IEnumerable<Budget>> GetBudgetsByMonthAsync(string monthYear);
        Task<Budget> CreateOrUpdateBudgetAsync(Budget budget);
        Task<bool> DeleteBudgetAsync(int id);
    }
}