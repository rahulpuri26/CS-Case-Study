using System;
using FManage.Exceptions;
using FManage.Model;
using FManage.Repositry; 
using NUnit.Framework.Internal.Builders;
namespace FManage.nUnitTests;

public class Tests
{

    //[TestFixture]
    public class Testing
    {
        private FinanceRepositoryImpl _financeRepository;

        [SetUp]
        public void Setup()
        {
            _financeRepository = new FinanceRepositoryImpl();
        }

        [Test]
        public void Test_CreateUser_Success()
        {
            
            var uniqueUsername = $"testuser_{Guid.NewGuid()}";  
            var uniqueEmail = $"testuser{Guid.NewGuid()}@example.com"; 

            var user = new User
            {
                Username = uniqueUsername,
                Password = "testpassword",
                Email = uniqueEmail
            };

            // Act
            bool result = _financeRepository.CreateUser(user);

            // Assert
            Assert.That(result, Is.True, "User should be created successfully.");
        }



        [Test]
        public void Test_CreateExpense_Success()
        {
            
            var expense = new Expense
            {
                UserId = 1, 
                Amount = 150.00m,
                CategoryId = 1, 
                Date = DateTime.Now,
                Description = "Test expense"
            };

            
            bool result = _financeRepository.CreateExpense(expense);

            
            Assert.That(result, Is.True, "Expense should be created successfully.");
        }

        [Test]
        public void Test_GetExpenseById_Success()
        {
            
            int expenseId = 1; 

           
            var expense = _financeRepository.GetAllExpenses(1).Find(e => e.ExpenseId == expenseId);

            
            Assert.That(expense, Is.Not.Null, "Expense should be found.");
            Assert.That(expense.ExpenseId, Is.EqualTo(expenseId), "The found expense ID should match the requested ID.");
        }

        [Test]
        public void Test_DeleteUser_Exception()
        {
            
            int invalidUserId = 999; 

            
            var ex = Assert.Throws<UserNotFoundException>(() => _financeRepository.DeleteUser(invalidUserId));
            Assert.That(ex.Message, Is.EqualTo($"User with ID {invalidUserId} not found."));
        }

        [Test]
        public void Test_DeleteExpense_Exception()
        {
            
            int invalidExpenseId = 999; 

            
            var ex = Assert.Throws<ExpenseNotFoundException>(() => _financeRepository.DeleteExpense(invalidExpenseId));
            Assert.That(ex.Message, Is.EqualTo($"Expense with ID {invalidExpenseId} not found."));
        }
    }
}

