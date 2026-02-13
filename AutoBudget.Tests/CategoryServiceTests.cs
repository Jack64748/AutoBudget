using Xunit;
using Microsoft.EntityFrameworkCore;
using AutoBudget_Backend.Models;
using AutoBudget_Backend.Services;
using System.Threading.Tasks;

namespace AutoBudget.Tests
{
    public class CategoryServiceTests
    {
        private BudgetContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<BudgetContext>()
                .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
                .Options;
            return new BudgetContext(options);
        }

        [Fact]
        public async Task DeleteCategory_ShouldResetTransactionsToOther()
        {
            var context = GetDbContext();
            var service = new CategoryService(context);
            
            context.Categories.Add(new Category { Id = 11, Name = "Other" });
            context.Categories.Add(new Category { Id = 1, Name = "Groceries" });
            
            context.Transactions.Add(new Transaction 
            { 
                Id = 1, 
                Description = "TESCO", 
                Currency = "Pounds",
                Product = "Pounds",
                State = "Pounds",
                Type =  "Pounds",
                CategoryId = 1, 
                 
            });
            await context.SaveChangesAsync();

            await service.DeleteCategoryAsync(1);

            var updated = await context.Transactions.FindAsync(1);
            Assert.Equal(11, updated.CategoryId);
            
            Assert.Null(await context.Categories.FindAsync(1));
        }
    }
}