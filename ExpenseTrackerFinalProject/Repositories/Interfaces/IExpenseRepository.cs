using ExpenseTracker.Models;

namespace ExpenseTracker.Repositories
{
    public interface IExpenseRepository
    {
        Task<List<Expense>> GetExpensesAsync();
        Task SaveExpensesAsync(List<Expense> expenses);
    }
}