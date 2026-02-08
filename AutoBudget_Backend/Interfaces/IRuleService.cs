using AutoBudget_Backend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoBudget_Backend.Services
{
    public interface IRuleService
    {
        Task<IEnumerable<CategoryRule>> GetAllRulesAsync();
        Task<CategoryRule?> CreateRuleAsync(CategoryRule rule);
        Task<bool> DeleteRuleAsync(int id);
    }
}
