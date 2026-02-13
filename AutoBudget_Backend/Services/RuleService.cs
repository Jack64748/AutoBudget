using Microsoft.EntityFrameworkCore;
using AutoBudget_Backend.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoBudget_Backend.Services
{
    public class RuleService : IRuleService
    {
        private readonly BudgetContext _context;

        public RuleService(BudgetContext context) => _context = context;

        public async Task<IEnumerable<CategoryRule>> GetAllRulesAsync() =>
            await _context.CategoryRules.Include(r => r.Category).OrderBy(r => r.Keyword).ToListAsync();

        public async Task<CategoryRule?> CreateRuleAsync(CategoryRule rule)
        {
            // 1. Normalize keyword
            if (rule.Keyword != null)
            {
                rule.Keyword = rule.Keyword.ToUpper();
            }

            // 2. Save the new rule
            _context.CategoryRules.Add(rule);
            await _context.SaveChangesAsync();

            // --- RETROACTIVE UPDATE LOGIC ---
            // 3. Find transactions that match the keyword and are currently in "Other" (ID 11)
            var matches = await _context.Transactions
                .Where(t => t.Description != null && 
                       t.Description.ToUpper().Contains(rule.Keyword) && 
                       (t.CategoryId == 11 || t.CategoryId == null))
                .ToListAsync();

            // 4. Update the CategoryId link only
            foreach (var t in matches)
            {
                t.CategoryId = rule.CategoryId;
            }

            await _context.SaveChangesAsync();
            // --------------------------------

            return await _context.CategoryRules
                .Include(r => r.Category)
                .FirstOrDefaultAsync(r => r.Id == rule.Id);
        }

        public async Task<bool> DeleteRuleAsync(int id)
        {
            var rule = await _context.CategoryRules.FindAsync(id);
            if (rule == null) return false;

            // 1. Find transactions categorized by this specific rule
            var affectedTransactions = await _context.Transactions
                .Where(t => t.Description != null && 
                       t.Description.ToUpper().Contains(rule.Keyword.ToUpper()) &&
                       t.CategoryId == rule.CategoryId)
                .ToListAsync();

            // 2. Reset those specific transactions back to "Other" (ID 11)
            foreach (var t in affectedTransactions)
            {
                t.CategoryId = 11;
            }

            _context.CategoryRules.Remove(rule);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}