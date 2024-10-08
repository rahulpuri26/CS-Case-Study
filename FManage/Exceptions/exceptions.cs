using System;
namespace FManage.Exceptions
{
    
     public class UserNotFoundException : Exception
        {
            public UserNotFoundException() : base("User not found!") { }

            public UserNotFoundException(string message) : base(message) { }

            public UserNotFoundException(string message, Exception innerException) : base(message, innerException) { }
        }

        // Custom Exception for Expense not found
        public class ExpenseNotFoundException : Exception
        {
            public ExpenseNotFoundException() : base("Expense not found!") { }

            public ExpenseNotFoundException(string message) : base(message) { }

            public ExpenseNotFoundException(string message, Exception innerException) : base(message, innerException) { }
        }
    }


