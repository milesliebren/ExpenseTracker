using ExpenseTracker.Services;
using ExpenseTracker.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(); // Enable MVC with Razor views

// Dependency Injection
builder.Services.AddScoped<IExpenseService, ExpenseService>();
builder.Services.Decorate<IExpenseService, ExpenseServiceDecorator>();

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

// Map controllers
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Expense}/{action=Index}/{id?}");

app.Run();
