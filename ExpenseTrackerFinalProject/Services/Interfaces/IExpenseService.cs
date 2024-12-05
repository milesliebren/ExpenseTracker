using ExpenseTracker.Models;

namespace ExpenseTracker.Services.Interfaces
{
    public interface IExpenseService
    {
        Task<List<Expense>> GetAllExpensesAsync();
        Task<Expense?> GetExpenseByIdAsync(int id);
        Task<Expense> AddExpenseAsync(Expense expense);
        Task<Expense?> UpdateExpenseAsync(Expense expense);
        Task<bool> DeleteExpenseAsync(int id);
    }
}
