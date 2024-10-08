using System;
using FManage.Model;

namespace FManage.Repositry.interfaces
{
        public interface IFinanceRepository
        {
            bool CreateUser(User user);
            bool CreateExpense(Expense expense);
           
            bool DeleteUser(int userId);
            bool DeleteExpense(int expenseId);
            List<ExpenseCategory> GetAllExpenseCategories();
            User GetUserByUsername(string username);
            bool UserExists(int userId);
            bool ExpenseExists(int expenseId);
        }
    }