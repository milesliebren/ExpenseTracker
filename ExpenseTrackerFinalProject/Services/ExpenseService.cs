using ExpenseTracker.Repositories;
using ExpenseTracker.Models;
using ExpenseTracker.Services.Interfaces;
using System.Text.Json;

namespace ExpenseTracker.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly string _filePath;

        // Default constructor uses the default file path
        public ExpenseService() : this("expenses.json") { }

        // Constructor for custom file path (e.g., for testing)
        public ExpenseService(string filePath)
        {
            _filePath = filePath;

            // Initialize JSON file if not already present
            if (!File.Exists(_filePath))
            {
                File.WriteAllText(_filePath, "[]");
            }
        }

        public async Task<List<Expense>> GetAllExpensesAsync()
        {
            var data = await File.ReadAllTextAsync(_filePath);
            return JsonSerializer.Deserialize<List<Expense>>(data) ?? new List<Expense>();
        }

        public async Task<Expense?> GetExpenseByIdAsync(int id)
        {
            var expenses = await GetAllExpensesAsync();
            return expenses.FirstOrDefault(e => e.Id == id);
        }

        public async Task<Expense> AddExpenseAsync(Expense expense)
        {
            var expenses = await GetAllExpensesAsync();
            expense.Id = expenses.Any() ? expenses.Max(e => e.Id) + 1 : 1; // Generate new ID for new expense

            expenses.Add(expense); // Add the expense to the list

            await SaveExpensesAsync(expenses); // Save the list of expenses (simulate saving to storage)
            return expense; // Return the added expense
        }

        public async Task<Expense?> UpdateExpenseAsync(Expense updatedExpense)
        {
            var expenses = await GetAllExpensesAsync();
            var existingExpense = expenses.FirstOrDefault(e => e.Id == updatedExpense.Id);

            if (existingExpense == null) return null;

            // Update fields
            existingExpense.Description = updatedExpense.Description;
            existingExpense.Amount = updatedExpense.Amount;
            existingExpense.Date = updatedExpense.Date;

            await SaveExpensesAsync(expenses);
            return existingExpense;
        }

        public async Task<bool> DeleteExpenseAsync(int id)
        {
            var expenses = await GetAllExpensesAsync();
            var expenseToDelete = expenses.FirstOrDefault(e => e.Id == id);

            if (expenseToDelete == null) return false;

            expenses.Remove(expenseToDelete);
            await SaveExpensesAsync(expenses);

            return true;
        }

        private async Task SaveExpensesAsync(List<Expense> expenses)
        {
            var data = JsonSerializer.Serialize(expenses);
            await File.WriteAllTextAsync(_filePath, data);
        }
    }
}
