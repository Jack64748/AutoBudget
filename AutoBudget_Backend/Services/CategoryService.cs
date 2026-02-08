using Microsoft.EntityFrameworkCore;
using AutoBudget_Backend.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoBudget_Backend.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly BudgetContext _context;

        public CategoryService(BudgetContext context) => _context = context;

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync() =>
            await _context.Categories.OrderBy(c => c.Id).ToListAsync();

        public async Task<Category?> GetCategoryByIdAsync(int id) =>
            await _context.Categories.FindAsync(id);

        public async Task<Category> CreateCategoryAsync(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<bool> UpdateCategoryAsync(int id, Category category)
        {
            if (id != category.Id) return false;
            _context.Entry(category).State = EntityState.Modified;
            try { await _context.SaveChangesAsync(); return true; }
            catch (DbUpdateConcurrencyException) { return false; }
        }

        public async Task<bool> DeleteCategoryAsync(int id)
{
    // Prevent deleting the "Other" category itself if you want to be safe
    if (id == 11) return false;

    var category = await _context.Categories
        .FirstOrDefaultAsync(c => c.Id == id);

    if (category == null) return false;

    // 1. Find all transactions linked to this category ID
    var affectedTransactions = await _context.Transactions
        .Where(t => t.CategoryId == id)
        .ToListAsync();

    // 2. Point them to "Other" (ID 11)
    foreach (var t in affectedTransactions)
    {
        t.CategoryId = 11;
        
    }

    _context.Categories.Remove(category);
    await _context.SaveChangesAsync();
    return true;
}
    }
}
