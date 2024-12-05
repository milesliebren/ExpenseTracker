using ExpenseTracker.Models;
using ExpenseTracker.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.UI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExpenseController : Controller
    {
        private readonly IExpenseService _expenseService;

        // Constructor for dependency injection
        public ExpenseController(IExpenseService expenseService)
        {
            _expenseService = expenseService;
        }

        /// <summary>
        /// Get all expenses.
        /// </summary>
        /// <returns>A list of all expenses.</returns>
        [HttpGet]
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

        /// <summary>
        /// Get a single expense by ID.
        /// </summary>
        /// <param name="id">The ID of the expense.</param>
        /// <returns>The expense if found.</returns>
        [HttpGet("{id:int}")]
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

        /// <summary>
        /// Add a new expense.
        /// </summary>
        /// <param name="expense">The expense data to add.</param>
        /// <returns>The created expense.</returns>
        [HttpPost]
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

        /// <summary>
        /// Update an existing expense.
        /// </summary>
        /// <param name="id">The ID of the expense to update.</param>
        /// <param name="expense">The updated expense data.</param>
        /// <returns>The updated expense.</returns>
        [HttpPut("{id:int}")]
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

        /// <summary>
        /// Delete an expense by ID.
        /// </summary>
        /// <param name="id">The ID of the expense to delete.</param>
        /// <returns>No content if successful.</returns>
        [HttpDelete("{id:int}")]
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

        // Index action method to load the view
        public async Task<IActionResult> Index()
        {
            var expenses = await _expenseService.GetAllExpensesAsync();
            return View(expenses); // Returns the Index view with the list of expenses
        }
    }
}
