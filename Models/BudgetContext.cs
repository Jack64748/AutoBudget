using Microsoft.EntityFrameworkCore;
using AutoBudget_Backend.Models;

// This is the "Translator." It uses Entity Framework Core (an Object Relational Mapper ORM) to turn 
// C# code into SQL commands that PostgreSQL understands.

// Inherits DbContext to have built in database powers, ability to open and close connections track changes and convert LINQ queries into SQL queries
public class BudgetContext : DbContext
{
    // options contains the configuration for database, such as Connection String (which is stored in appsettings.json)
    // base(options) passes those settings to the main Entity Framework system so it knows exactly which PostgreSQL server to talk to.
    public BudgetContext(DbContextOptions<BudgetContext> options) : base(options) { }

    // The table definition This maps directly to a table in PostgreSQL
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<CategoryRule> CategoryRules { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    // Seed some initial data so the app works immediately
    modelBuilder.Entity<Category>().HasData(
        new Category { Id = 1, Name = "Groceries" },
        new Category { Id = 2, Name = "Fitness" },
        new Category { Id = 3, Name = "Rent" },
        new Category { Id = 4, Name = "Phone" },
        new Category { Id = 5, Name = "Amazon" },
        new Category { Id = 6, Name = "Accomadation" },
        new Category { Id = 7, Name = "Transport" },
        new Category { Id = 8, Name = "Savings" },
        new Category { Id = 9, Name = "Income" },
        new Category { Id = 10, Name = "Transfer" },
        new Category { Id = 11, Name = "Other" }
    );

    modelBuilder.Entity<CategoryRule>().HasData(
        new CategoryRule { Id = 1, Keyword = "TESCO", CategoryId = 1 },
        new CategoryRule { Id = 2, Keyword = "ASDA", CategoryId = 1 },
        new CategoryRule { Id = 3, Keyword = "LIDL", CategoryId = 1 },
        new CategoryRule { Id = 4, Keyword = "ALDI", CategoryId = 1 },
        new CategoryRule { Id = 5, Keyword = "SAINSBURY'S", CategoryId = 1 },
        new CategoryRule { Id = 6, Keyword = "SUPERVALU", CategoryId = 1 },
        new CategoryRule { Id = 7, Keyword = "DUNNES STORES", CategoryId = 1 },
        new CategoryRule { Id = 8, Keyword = "CO-OP", CategoryId = 1 },
        new CategoryRule { Id = 9, Keyword = "CENTRA", CategoryId = 1 },
        new CategoryRule { Id = 10, Keyword = "ASDA", CategoryId = 1 },
        new CategoryRule { Id =11, Keyword = "FITNESS", CategoryId = 2 },
        new CategoryRule { Id = 12, Keyword = "GYM", CategoryId = 2 },
        new CategoryRule { Id = 13, Keyword = "AMAZON", CategoryId = 5 },
        new CategoryRule { Id =14, Keyword = "RENT", CategoryId = 3 },
        new CategoryRule { Id = 15, Keyword = "VODAFONE", CategoryId = 4 },
        new CategoryRule { Id = 16, Keyword = "EIR", CategoryId = 4 },
        new CategoryRule { Id = 17, Keyword = "TESCOMOBILE", CategoryId = 4 },
        new CategoryRule { Id = 18, Keyword = "THREE", CategoryId = 4 },
        new CategoryRule { Id = 19, Keyword = "VIRGINMEDIA", CategoryId = 4 },
        new CategoryRule { Id = 20, Keyword = "HOTEL", CategoryId = 6 },
        new CategoryRule { Id = 21, Keyword = "HOSTEL", CategoryId = 6 },
        new CategoryRule { Id = 22, Keyword = "BUS", CategoryId = 7 },
        new CategoryRule { Id = 23, Keyword = "TRANSLINK", CategoryId = 7 },
        new CategoryRule { Id = 24, Keyword = "RYANAIR", CategoryId = 7 },
        new CategoryRule { Id = 25, Keyword = "FERRY", CategoryId = 7 },
        new CategoryRule { Id = 26, Keyword = "UBER", CategoryId = 7 },
        new CategoryRule { Id = 27, Keyword = "SAVINGS", CategoryId = 8 },
        new CategoryRule { Id = 28, Keyword = "PAYMENT", CategoryId = 9 },
        new CategoryRule { Id = 29, Keyword = "TRANSFER FROM", CategoryId = 9 },
        new CategoryRule { Id = 30, Keyword = "TRANSFER TO", CategoryId = 10 }
        
    );
}
}
