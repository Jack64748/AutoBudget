using Microsoft.EntityFrameworkCore;
using AutoBudget_Backend.Models;

public class BudgetContext : DbContext
{
    public BudgetContext(DbContextOptions<BudgetContext> options) : base(options) { }

    public DbSet<Transaction> Transactions { get; set; }
}
