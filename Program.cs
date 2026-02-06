using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using AutoBudget_Backend.Services; 

// fix for PostgreSQL (Npgsql) postgre is strict about DateTime this switch tells system to be a bit more flexible preventing the app from crashing
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// Begins registering all the tools in the app before it starts running 
var builder = WebApplication.CreateBuilder(args);

// CORS (Cross-Origin Resource Sharing) is a security feature.
// By default, browsers block a website at localhost:3000 (React) from talking to a server at localhost:5170 (.NET).
// This code makes a policy that allows react to speak to the backend server
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// this is a service registration line tells .net to look for any class inheriting from ControllerBase
builder.Services.AddControllers();

// These create swagger the api's automatic documentation
// Goes through code to find every single URL/endpoint and creates a map of the api
builder.Services.AddEndpointsApiExplorer();
// Takes the map and makes a interactive webpage allows to test backend without react code
builder.Services.AddSwaggerGen();

// tells .net to use budgetcontext and appsettings.json to find the postgresql server
builder.Services.AddDbContext<BudgetContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("BudgetDb")));
// this is the dependency injection registration 
// tells app if controller asks for ibudgetservice give it a new instance of budgetservice
builder.Services.AddScoped<IBudgetService, BudgetService>();

// creates the application
var app = builder.Build();

// sequence of steps every request goes through 
app.UseCors();

// acts as a switchboard operator when a request comes in it looks at the url and matches it to the correct controller
app.UseRouting();

// generates a manual for api to test features without even using react
app.UseSwagger();
app.UseSwaggerUI();

// activates the security clearance from earlier
app.UseCors("AllowAll");

// It checks if the person making the request has permission to access the resource.
app.UseAuthorization();

// This maps the incoming URLs (like /api/Budget) to the actual code in BudgetController.cs.
app.MapControllers();

// This starts the server and tells it to "listn" specifically on Port 5170
app.Run("http://localhost:5170");