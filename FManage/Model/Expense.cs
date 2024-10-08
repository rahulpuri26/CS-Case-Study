using System;
namespace FManage.Model
{
    public class Expense
    {
        public int ExpenseId { get; set; }
        public int UserId { get; set; }
        public decimal Amount { get; set; }
        public int CategoryId { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }

        public Expense() { }

        public Expense(int expenseId, int userId, decimal amount, int categoryId, DateTime date, string description)
        {
            ExpenseId = expenseId;
            UserId = userId;
            Amount = amount;
            CategoryId = categoryId;
            Date = date;
            Description = description;
        }
    }
}
