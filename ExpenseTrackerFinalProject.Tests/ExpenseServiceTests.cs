using ExpenseTracker.Models;
using ExpenseTracker.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker.Tests
{
    [TestClass]
    public class ExpenseServiceTests
    {
        private ExpenseService _expenseService;

        [TestInitialize]
        public void Initialize()
        {
            // Set up a JSON file for test data
            string testJsonFile = "test_expenses.json";
            if (File.Exists(testJsonFile))
                File.Delete(testJsonFile);

            File.WriteAllText(testJsonFile, "[]"); // Empty JSON array for initialization
            _expenseService = new ExpenseService(testJsonFile);
        }

        [TestCleanup]
        public void Cleanup()
        {
            // Clean up test JSON file
            string testJsonFile = "test_expenses.json";
            if (File.Exists(testJsonFile))
                File.Delete(testJsonFile);
        }

        [TestMethod]
        public async Task AddExpenseAsync_ShouldAddExpense()
        {
            // Arrange
            var expense = new Expense { Description = "Test Expense", Amount = 50, Date = System.DateTime.Now };

            // Act
            var result = await _expenseService.AddExpenseAsync(expense);
            var allExpenses = await _expenseService.GetAllExpensesAsync();

            // Assert
            Assert.AreEqual(1, allExpenses.Count);
            Assert.AreEqual("Test Expense", allExpenses.First().Description);
        }

        [TestMethod]
        public async Task GetAllExpensesAsync_ShouldReturnAllExpenses()
        {
            // Arrange
            var expense1 = new Expense { Description = "Expense 1", Amount = 100, Date = System.DateTime.Now };
            var expense2 = new Expense { Description = "Expense 2", Amount = 200, Date = System.DateTime.Now };
            await _expenseService.AddExpenseAsync(expense1);
            await _expenseService.AddExpenseAsync(expense2);

            // Act
            var result = await _expenseService.GetAllExpensesAsync();

            // Assert
            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public async Task GetExpenseByIdAsync_ShouldReturnCorrectExpense()
        {
            // Arrange
            var expense = new Expense { Description = "Find Me", Amount = 30, Date = System.DateTime.Now };
            var addedExpense = await _expenseService.AddExpenseAsync(expense);

            // Act
            var result = await _expenseService.GetExpenseByIdAsync(addedExpense.Id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Find Me", result.Description);
        }

        [TestMethod]
        public async Task UpdateExpenseAsync_ShouldUpdateExpense()
        {
            // Arrange
            var expense = new Expense { Description = "Update Me", Amount = 70, Date = System.DateTime.Now };
            var addedExpense = await _expenseService.AddExpenseAsync(expense);
            addedExpense.Description = "Updated";

            // Act
            var result = await _expenseService.UpdateExpenseAsync(addedExpense);
            var updatedExpense = await _expenseService.GetExpenseByIdAsync(addedExpense.Id);

            // Assert
            Assert.AreEqual("Updated", updatedExpense.Description);
        }

        [TestMethod]
        public async Task DeleteExpenseAsync_ShouldRemoveExpense()
        {
            // Arrange
            var expense = new Expense { Description = "Remove Me", Amount = 90, Date = System.DateTime.Now };
            var addedExpense = await _expenseService.AddExpenseAsync(expense);

            // Act
            var result = await _expenseService.DeleteExpenseAsync(addedExpense.Id);
            var allExpenses = await _expenseService.GetAllExpensesAsync();

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(0, allExpenses.Count);
        }
    }
}
