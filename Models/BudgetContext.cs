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
        new Category { Id = 6, Name = "Other" }
    );

    modelBuilder.Entity<CategoryRule>().HasData(
        new CategoryRule { Id = 1, Keyword = "TESCO", CategoryId = 1 },
        new CategoryRule { Id = 2, Keyword = "ASDA", CategoryId = 1 },
        new CategoryRule { Id = 3, Keyword = "FITNESS", CategoryId = 2 },
        new CategoryRule { Id = 4, Keyword = "AMAZON", CategoryId = 5 },
        new CategoryRule { Id = 5, Keyword = "LIDL", CategoryId = 1 },
        new CategoryRule { Id = 6, Keyword = "GYM", CategoryId = 2 }
    );
}
}
