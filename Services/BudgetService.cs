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

    // Opens a stream to read the file line by line instead of load whole thing
    using var reader = new StreamReader(file.OpenReadStream());
    // Tells code to forgive if header slighlty off or field is mising
    var config = new CsvConfiguration(CultureInfo.InvariantCulture)
    {
        HeaderValidated = null,
        MissingFieldFound = null,
    };



    //Creates new instance of CsvHelper engine uses reader and config from above for stream of text and settings
    using var csv = new CsvReader(reader, config);
    // Tells reader to use my custom TransactionMap class makes sure Started Date gets saved to StartedDate in my C# model
    csv.Context.RegisterClassMap<TransactionMap>();
    // Library reads every line of csv converts the text strings into numbers and dates and makes a new transaction object for each row
    // Stores the entire collection into a list in memory
    var records = csv.GetRecords<Transaction>().ToList();




    // Convert parsed dates to UTC manually stamps each date as UTC tells postgreSQL time is UTC and is safe to save
    foreach (var record in records)
    {
        if (record.StartedDate.HasValue)
            record.StartedDate = DateTime.SpecifyKind(record.StartedDate.Value, DateTimeKind.Utc);
            
        if (record.CompletedDate.HasValue)
            record.CompletedDate = DateTime.SpecifyKind(record.CompletedDate.Value, DateTimeKind.Utc);
    }
    // Prepares entire list to be sent to database in a single batch 
    await _context.Transactions.AddRangeAsync(records);
    // Opens connection to PostgreSQL and executes the SQL INSERT statement
    await _context.SaveChangesAsync();
    // Queries the entire database for the entire list of transactions to ensure frontend always up to date including any previous transactions
    return await _context.Transactions.ToListAsync();
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
    // sends a SELECT * FROM "Transactions" command to PostgreSQL database, await ensures backend doesnt freeze while database searches
    return await _context.Transactions.ToListAsync();
}
}


// This class tells the CsvHelper library how to read specific CSV file
public sealed class TransactionMap : ClassMap<Transaction>
{
    public TransactionMap()
    {
        // Ignore id column as database generates id automatically
        AutoMap(CultureInfo.InvariantCulture);
        Map(m => m.Id).Ignore();

        // bridges gap between Started Date and StartedDate and tells code how to read uk vs us date formats
        Map(m => m.StartedDate)
            .Name("Started Date")
            .TypeConverterOption.Format("yyyy-MM-dd HH:mm:ss");

        Map(m => m.CompletedDate)
            .Name("Completed Date")
            .TypeConverterOption.Format("yyyy-MM-dd HH:mm:ss");


        // This tells CsvHelper: "If the text is empty or whitespace, set it to null instead of set as a number which would cause a crash"
        Map(m => m.Amount).Name("Amount").TypeConverterOption.NullValues("");
        Map(m => m.Fee).Name("Fee").TypeConverterOption.NullValues("");
        Map(m => m.Balance).Name("Balance").TypeConverterOption.NullValues("");
    }
}
