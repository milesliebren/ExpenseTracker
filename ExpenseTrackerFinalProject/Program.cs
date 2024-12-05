using ExpenseTracker.Models;
using ExpenseTracker.Repositories;
using ExpenseTracker.Services;
using ExpenseTracker.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Register services for Dependency Injection
builder.Services.AddControllersWithViews(); // Add MVC with Razor Views

// Add the necessary services for Expense tracking
builder.Services.AddScoped<IExpenseService, ExpenseService>();
builder.Services.Decorate<IExpenseService, ExpenseServiceDecorator>();
builder.Services.AddSingleton<ExpenseRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

// Map the default route to HomeController's Index action
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Expense}/{action=Index}/{id?}"); // Default route points to ExpenseController and its Index method

// Map the controller to specific actions
app.MapControllers();

app.Run();
