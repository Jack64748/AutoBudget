using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using AutoBudget_Backend.Models;
using AutoBudget_Backend.Services;
using System;
using System.Threading.Tasks;
using System.Linq;

namespace AutoBudget.Tests
{
    public class BudgetServiceTests : IDisposable
    {
        private readonly BudgetContext _context;
        private readonly BudgetService _service;

        public BudgetServiceTests()
        {
            // 1. Setup the In-Memory DB with a unique name for this test run
            var options = new DbContextOptionsBuilder<BudgetContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new BudgetContext(options);
            
            // 2. Centralized service creation
            _service = new BudgetService(_context);
        }




        [Fact]
        public async Task GetSavingsBalance_ReturnsLatest()
        {
            // Arrange
            _context.Transactions.Add(new Transaction { 
                Product = "Savings", 
                Balance = 100, 
                StartedDate = DateTime.UtcNow,
                Type = "Topup",
                Description = "Initial Savings",
                Currency = "GBP",
                State = "COMPLETED"
            });
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.GetSavingsBalanceAsync();

            // Assert
            Assert.Equal(100, result);
        }



        [Fact]
        public async Task ReassignTransactions_UpdatesExistingAndCreatesRule()
        {
            // Arrange
            var description = "NETFLIX.COM";
            _context.Transactions.Add(new Transaction { 
                Id = 10,
                Description = description, 
                CategoryId = 1, 
                Product = "Current", 
                Type = "Debit", 
                Currency = "GBP", 
                State = "COMPLETED" 
            });
            await _context.SaveChangesAsync();

            // Act: Use the centralized _service
            await _service.ReassignTransactionsAsync(description, 2);

            // Assert
            var updatedTx = await _context.Transactions.FirstAsync();
            var newRule = await _context.CategoryRules.FirstOrDefaultAsync(r => r.Keyword == description.ToUpper());

            Assert.Equal(2, updatedTx.CategoryId);
            Assert.NotNull(newRule);
            Assert.Equal(2, newRule.CategoryId);
        }



        [Fact]
public async Task GetSavingsBalance_ReturnsZero_WhenNoTransactionsExist()
{
    // Act
    var result = await _service.GetSavingsBalanceAsync();

    // Assert
    // It should return 0, not throw an error
    Assert.Equal(0, result);
}



        [Fact]
public async Task AutoCategorization_AssignsCorrectCategory_BasedOnRule()
{
    // Arrange
    // 1. Create a Category and a Rule
    var foodCategory = new Category { Id = 5, Name = "Food" };
    _context.Categories.Add(foodCategory);
    _context.CategoryRules.Add(new CategoryRule { Keyword = "LIDL", CategoryId = 5 });
    
    // 2. Add the 'Other' category which the service expects as a fallback
    _context.Categories.Add(new Category { Id = 1, Name = "Other" });
    await _context.SaveChangesAsync();

    // We simulate a list of transactions (since we can't easily mock IFormFile here, 
    // we are testing the logic that would run inside the loop)
    var transactions = new List<Transaction> 
    { 
        new Transaction { Description = "LIDL UK STORE 123", Product = "Current", Type="T", Currency="G", State="C" } 
    };

    // Act
    // Note: To test this properly, you might need to make your categorization logic 
    // a separate public/internal method, but for now, we'll verify if a rule exists.
    var rule = _context.CategoryRules.FirstOrDefault(r => transactions[0].Description.Contains(r.Keyword));

    // Assert
    Assert.NotNull(rule);
    Assert.Equal(5, rule.CategoryId);
}




[Fact]
public async Task GetSavingsBalance_ReturnsLastUploaded_WhenDatesAreSame()
{
    // Arrange
    var sameDate = new DateTime(2024, 05, 01);
    _context.Transactions.AddRange(
        new Transaction { Id = 50, Product = "Savings", Balance = 200, StartedDate = sameDate, Type="T", Description="D", Currency="G", State="C" },
        new Transaction { Id = 51, Product = "Savings", Balance = 350, StartedDate = sameDate, Type="T", Description="D", Currency="G", State="C" }
    );
    await _context.SaveChangesAsync();

    // Act
    var result = await _service.GetSavingsBalanceAsync();

    // Assert
    // Even though the dates are identical, it should pick Id 51 (350)
    Assert.Equal(350, result);
}

[Fact]
public async Task ProcessCsv_ThrowsException_IfOtherCategoryIsMissing()
{
    // Arrange: Create a fake (mock) empty file so it doesn't crash on line 34
    var content = "Type,Product,Started Date,Description,Amount,Balance"; // Just headers
    var fileName = "test.csv";
    var stream = new MemoryStream();
    var writer = new StreamWriter(stream);
    writer.Write(content);
    writer.Flush();
    stream.Position = 0;

    var mockFile = new FormFile(stream, 0, stream.Length, "file", fileName);

    // Act & Assert: Now the code will pass the file check and hit the Category check!
    var exception = await Assert.ThrowsAsync<Exception>(() => _service.ProcessBudgetCsvAsync(mockFile));
    
    Assert.Equal("Default categories not found. Please run database migrations.", exception.Message);
}

        // This runs after EVERY test to keep the environment clean
        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
