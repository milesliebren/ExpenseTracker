using ExpenseTracker.Models;
using System.Text.Json;

namespace ExpenseTracker.Repositories
{
    public class ExpenseRepository : IExpenseRepository
    {
        private const string FilePath = "expenses.json";

        public async Task<List<Expense>> GetExpensesAsync()
        {
            if (!File.Exists(FilePath))
                return new List<Expense>();

            string json = await File.ReadAllTextAsync(FilePath);
            return JsonSerializer.Deserialize<List<Expense>>(json) ?? new List<Expense>();
        }

        public async Task SaveExpensesAsync(List<Expense> expenses)
        {
            string json = JsonSerializer.Serialize(expenses);
            await File.WriteAllTextAsync(FilePath, json);
        }
    }
}