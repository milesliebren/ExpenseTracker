using ExpenseTracker.Models;
using ExpenseTracker.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Controllers
{
    public class ExpenseController : Controller
    {
        private readonly IExpenseService _expenseService;

        // Constructor for dependency injection
        public ExpenseController(IExpenseService expenseService)
        {
            _expenseService = expenseService;
        }

        // API endpoint to get all expenses
        [HttpGet("api/expenses")]
        public async Task<IActionResult> GetAllExpenses()
        {
            try
            {
                var expenses = await _expenseService.GetAllExpensesAsync();
                return Ok(expenses);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // API endpoint to get an expense by its ID
        [HttpGet("api/expenses/{id:int}")]
        public async Task<IActionResult> GetExpenseById(int id)
        {
            try
            {
                var expense = await _expenseService.GetExpenseByIdAsync(id);

                if (expense == null)
                    return NotFound($"Expense with ID {id} not found.");

                return Ok(expense);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // API endpoint to add a new expense
        [HttpPost("api/expenses")]
        public async Task<IActionResult> AddExpense([FromBody] Expense expense)
        {
            try
            {
                if (expense == null)
                    return BadRequest("Expense data is required.");

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var createdExpense = await _expenseService.AddExpenseAsync(expense);

                return CreatedAtAction(nameof(GetExpenseById), new { id = createdExpense.Id }, createdExpense);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // API endpoint to update an expense
        [HttpPut("api/expenses/{id:int}")]
        public async Task<IActionResult> UpdateExpense(int id, [FromBody] Expense expense)
        {
            try
            {
                if (expense == null || expense.Id != id)
                    return BadRequest("Expense data is invalid.");

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var updatedExpense = await _expenseService.UpdateExpenseAsync(expense);

                if (updatedExpense == null)
                    return NotFound($"Expense with ID {id} not found.");

                return Ok(updatedExpense);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // API endpoint to delete an expense
        [HttpDelete("api/expenses/{id:int}")]
        public async Task<IActionResult> DeleteExpense(int id)
        {
            try
            {
                var isDeleted = await _expenseService.DeleteExpenseAsync(id);

                if (!isDeleted)
                    return NotFound($"Expense with ID {id} not found.");

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // MVC action method to load the Index view
        public async Task<IActionResult> Index()
        {
            var expenses = await _expenseService.GetAllExpensesAsync();
            return View(expenses); // Return the Index view with the list of expenses
        }
    }
}
