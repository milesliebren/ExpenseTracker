﻿using ExpenseTracker.Models;
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

        // MVC action method to load the Index view
        public async Task<IActionResult> Index()
        {
            var expenses = await _expenseService.GetAllExpensesAsync();
            return View(expenses); // Return the Index view with the list of expenses
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Expense expense)
        {
            if (ModelState.IsValid)
            {
                var createdExpense = await _expenseService.AddExpenseAsync(expense);
                return RedirectToAction("Index");
            }
            return View(expense);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var expense = await _expenseService.GetExpenseByIdAsync(id);
            if (expense == null)
                return NotFound();

            return View(expense);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Expense expense)
        {
            if (ModelState.IsValid)
            {
                var updatedExpense = await _expenseService.UpdateExpenseAsync(expense);
                return RedirectToAction("Index");
            }
            return View(expense);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var expense = await _expenseService.GetExpenseByIdAsync(id);
            if (expense == null)
                return NotFound();

            return View(expense);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Expense expense)
        {
            var isDeleted = await _expenseService.DeleteExpenseAsync(expense.Id);
            if (isDeleted)
                return RedirectToAction("Index");
            return NotFound();
        }
    }
}
