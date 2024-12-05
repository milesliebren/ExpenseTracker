using ExpenseTracker.Models;
using ExpenseTracker.Services.Interfaces;

namespace ExpenseTracker.Services
{
    public class ExpenseServiceDecorator : IExpenseService
    {
        private readonly IExpenseService _decoratedService;

        public ExpenseServiceDecorator(IExpenseService decoratedService)
        {
            _decoratedService = decoratedService;
        }

        public async Task<List<Expense>> GetAllExpensesAsync()
        {
            // Logging or additional functionality can be added here
            Console.WriteLine("Fetching all expenses...");
            return await _decoratedService.GetAllExpensesAsync();
        }

        public async Task<Expense?> GetExpenseByIdAsync(int id)
        {
            // Logging or additional functionality
            Console.WriteLine($"Fetching expense with ID {id}...");
            return await _decoratedService.GetExpenseByIdAsync(id);
        }

        public async Task<Expense> AddExpenseAsync(Expense expense)
        {
            // Logging or additional functionality
            Console.WriteLine($"Adding a new expense: {expense.Description}, Amount: {expense.Amount}, Date: {expense.Date}");
            return await _decoratedService.AddExpenseAsync(expense);
        }

        public async Task<Expense?> UpdateExpenseAsync(Expense expense)
        {
            // Logging or additional functionality
            Console.WriteLine($"Updating expense ID {expense.Id}...");
            return await _decoratedService.UpdateExpenseAsync(expense);
        }

        public async Task<bool> DeleteExpenseAsync(int id)
        {
            // Logging or additional functionality
            Console.WriteLine($"Deleting expense with ID {id}...");
            return await _decoratedService.DeleteExpenseAsync(id);
        }
    }
}

