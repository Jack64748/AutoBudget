using AutoBudget_Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace AutoBudget_Backend.Services
{
    public class TheBudgetService : ITheBudgetService
    {
        private readonly BudgetContext _context;

        public TheBudgetService(BudgetContext context) => _context = context;

        public async Task<IEnumerable<Budget>> GetBudgetsByMonthAsync(string monthYear)
        {
            return await _context.Budgets
                .Include(b => b.Category)
                .Where(b => b.MonthYear == monthYear)
                .ToListAsync();
        }

        public async Task<Budget> CreateOrUpdateBudgetAsync(Budget budget)
        {
            var existing = await _context.Budgets
                .FirstOrDefaultAsync(b => b.CategoryId == budget.CategoryId && b.MonthYear == budget.MonthYear);

            int finalId; // We will store the ID here first

            if (existing != null)
            {
                existing.Amount = budget.Amount;
                _context.Budgets.Update(existing);
                await _context.SaveChangesAsync();
                finalId = existing.Id;
            }
            else
            {
                _context.Budgets.Add(budget);
                await _context.SaveChangesAsync();
                finalId = budget.Id;
            }

            // Now the query is simple and EF can translate it to SQL perfectly
            return await _context.Budgets
                .Include(b => b.Category)
                .FirstAsync(b => b.Id == finalId);
        }

        public async Task<bool> DeleteBudgetAsync(int id)
        {
            var budget = await _context.Budgets.FindAsync(id);
            if (budget == null) return false;

            _context.Budgets.Remove(budget);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}