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
}
