using AutoBudget_Backend.Models;
using CsvHelper;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using CsvHelper.Configuration;
using System.Globalization;

public class BudgetService : IBudgetService
{
    private readonly BudgetContext _context;

    public BudgetService(BudgetContext context)
    {
        _context = context;
    }

    public async Task<List<Transaction>> ProcessBudgetCsvAsync(IFormFile file)
{
    using var reader = new StreamReader(file.OpenReadStream());
    
    var config = new CsvConfiguration(CultureInfo.InvariantCulture)
    {
        HeaderValidated = null,
        MissingFieldFound = null,
    };

    using var csv = new CsvReader(reader, config);
    csv.Context.RegisterClassMap<TransactionMap>();
    
    var records = csv.GetRecords<Transaction>().ToList();

    // MANDATORY FOR POSTGRES: Convert parsed dates to UTC
    foreach (var record in records)
    {
        if (record.StartedDate.HasValue)
            record.StartedDate = DateTime.SpecifyKind(record.StartedDate.Value, DateTimeKind.Utc);
            
        if (record.CompletedDate.HasValue)
            record.CompletedDate = DateTime.SpecifyKind(record.CompletedDate.Value, DateTimeKind.Utc);
    }

    await _context.Transactions.AddRangeAsync(records);
    await _context.SaveChangesAsync();

    return await _context.Transactions.ToListAsync();
}
    public async Task ClearAllTransactionsAsync()
{
    // This is the fastest way to empty a table in EF Core
    await _context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE \"Transactions\" RESTART IDENTITY");
}

    public async Task<List<Transaction>> GetAllTransactionsAsync()
{
    return await _context.Transactions.ToListAsync();
}
}



public sealed class TransactionMap : ClassMap<Transaction>
{
    public TransactionMap()
    {
        AutoMap(CultureInfo.InvariantCulture);
        Map(m => m.Id).Ignore();

        // Strictly use the Year-Month-Day format
        Map(m => m.StartedDate)
            .Name("Started Date")
            .TypeConverterOption.Format("yyyy-MM-dd HH:mm:ss");

        Map(m => m.CompletedDate)
            .Name("Completed Date")
            .TypeConverterOption.Format("yyyy-MM-dd HH:mm:ss");


        // This tells CsvHelper: "If the text is empty or whitespace, set it to null"
        Map(m => m.Amount).Name("Amount").TypeConverterOption.NullValues("");
        Map(m => m.Fee).Name("Fee").TypeConverterOption.NullValues("");
        Map(m => m.Balance).Name("Balance").TypeConverterOption.NullValues("");
    }
}
