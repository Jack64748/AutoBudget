using AutoBudget_Backend.Models;
using CsvHelper;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using CsvHelper.Configuration;
using System.Globalization;

public class BudgetService : IBudgetService
{

    // Receives Budget Context through constructor allows service to communicate to PostgreSQL
    // marking it _context it can be used in every method in this class to save or retrieve data
    private readonly BudgetContext _context;

    public BudgetService(BudgetContext context)
    {
        _context = context;
    }


    // Handles raw file into organised database rows
    public async Task<List<Transaction>> ProcessBudgetCsvAsync(IFormFile file)
{

    var config = new CsvConfiguration(CultureInfo.InvariantCulture)
{
    PrepareHeaderForMatch = args => args.Header.Trim(), // Handles extra spaces in CSV headers
    HeaderValidated = null, // This is a safety net: it won't crash if a header is missing
    MissingFieldFound = null // Won't crash if a specific row is missing a value
};

    using var reader = new StreamReader(file.OpenReadStream());
    using var csv = new CsvReader(reader, config);
    csv.Context.RegisterClassMap<TransactionMap>();
    
    var records = csv.GetRecords<Transaction>().ToList();

    // 1. Fetch all rules and categories from DB
    var rules = await _context.CategoryRules.ToListAsync();
    var otherCategory = await _context.Categories.FirstOrDefaultAsync(c => c.Name == "Other");

    // FAIL-SAFE: If 'Other' doesn't exist in DB yet, create it or use a default ID
    if (otherCategory == null)
    {
    // Optionally create it here or just ensure your Seed Data is working
    throw new Exception("Default categories not found. Please run database migrations.");
    }

    foreach (var record in records)
    {
        // 2. Fix Dates for Postgres
        if (record.StartedDate.HasValue)
            record.StartedDate = DateTime.SpecifyKind(record.StartedDate.Value, DateTimeKind.Utc);

        // 3. AUTO-CATEGORIZATION LOGIC
        // We look for a match between the description and our keywords
        var match = rules.FirstOrDefault(r => 
            record.Description != null && 
            record.Description.ToUpper().Contains(r.Keyword.ToUpper()));

        if (match != null)
        {
            record.CategoryId = match.CategoryId;
            record.Category = null;
        }
        else
        {
            record.CategoryId = otherCategory.Id;  
            record.Category = null; 
        }
    }

    await _context.Transactions.AddRangeAsync(records);
    await _context.SaveChangesAsync();

    return await _context.Transactions.Include(t => t.Category).ToListAsync();
}



public async Task ReassignTransactionsAsync(string description, int newCategoryId)
{
    // 1. Find all transactions with this exact description
    var transactions = await _context.Transactions
        .Where(t => t.Description == description)
        .ToListAsync();

    // 2. Update them
    foreach (var t in transactions)
    {
        t.CategoryId = newCategoryId;
    }

    // 3. Create or Update the Rule
    var keyword = description.ToUpper();
    var existingRule = await _context.CategoryRules
        .FirstOrDefaultAsync(r => r.Keyword == keyword);

    if (existingRule == null)
    {
        _context.CategoryRules.Add(new CategoryRule 
        {
            Keyword = keyword,
            CategoryId = newCategoryId
        });
    }
    else
    {
        existingRule.CategoryId = newCategoryId;
    }

    await _context.SaveChangesAsync();
}


    public async Task ClearAllTransactionsAsync()
{
    // This is the fastest way to empty a table in EF Core
    // TRUNCATE drops the data instantly instead of row one by one
    // \"Transactions\" is the table name
    // RESTART IDENTITY resets id counter back to 1 and not 1001
    await _context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE \"Transactions\" RESTART IDENTITY");
}






    public async Task<List<Transaction>> GetAllTransactionsAsync()
{
    
    return await _context.Transactions
        .Include(t => t.Category) 
        .OrderByDescending(t => t.StartedDate) // Added sorting so newest is at the top
        .ToListAsync();
}
}


// This class tells the CsvHelper library how to read specific CSV file
public sealed class TransactionMap : ClassMap<Transaction>
{
    public TransactionMap()
    {
        // Map only the columns that exist in the physical CSV file
        Map(m => m.Type).Name("Type");
        Map(m => m.Product).Name("Product");
        
        Map(m => m.StartedDate)
            .Name("Started Date")
            .TypeConverterOption.Format("yyyy-MM-dd HH:mm:ss");

        Map(m => m.CompletedDate)
            .Name("Completed Date")
            .TypeConverterOption.Format("yyyy-MM-dd HH:mm:ss");

        Map(m => m.Description).Name("Description");

        Map(m => m.Amount).Name("Amount").TypeConverterOption.NullValues("");
        Map(m => m.Fee).Name("Fee").TypeConverterOption.NullValues("");
        Map(m => m.Currency).Name("Currency");
        Map(m => m.State).Name("State");
        Map(m => m.Balance).Name("Balance").TypeConverterOption.NullValues("");

        // EXPLICITLY IGNORE everything else so CsvHelper doesn't get confused
        Map(m => m.Id).Ignore();
        Map(m => m.CategoryId).Ignore();
        Map(m => m.Category).Ignore();
    }
}
