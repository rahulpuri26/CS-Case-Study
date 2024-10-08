using System;
using System.Collections.Generic;
using ConsoleTables;
using FManage.Exceptions;
using FManage.Model;
using FManage.Repositry;

namespace FManage.MainMenu
{
    public class menu
    {
        private FinanceRepositoryImpl _financeRepository;

        public menu()
        {
            _financeRepository = new FinanceRepositoryImpl();
        }

        
        public void ShowWelcomeMenu()
        {
            bool exit = false;

            while (!exit)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\nWelcome to the Finance Management System");
                Console.ResetColor();
                Console.WriteLine("1. Log In");
                Console.WriteLine("2. Register as New User");
                Console.WriteLine("3. Exit");
                Console.Write("Choose an option: ");

                if (!int.TryParse(Console.ReadLine(), out int option))
                {
                    Console.WriteLine("Invalid input! Please enter a valid number.");
                    continue;
                }

                switch (option)
                {
                    case 1:
                        if (Login())
                        {
                            ShowMainMenu();  
                        }
                        break;
                    case 2:
                        RegisterNewUser();
                        break;
                    case 3:
                        exit = true;
                        Console.WriteLine("Exiting the application...");
                        break;
                    default:
                        Console.WriteLine("Invalid option! Please choose a valid menu option.");
                        break;
                }
            }
        }

        
        private void RegisterNewUser()
        {
            Console.WriteLine("\nRegister as a New User");

            Console.Write("Enter Username: ");
            string username = Console.ReadLine();
            Console.Write("Enter Password: ");
            string password = Console.ReadLine();
            Console.Write("Enter Email: ");
            string email = Console.ReadLine();

            User user = new User
            {
                Username = username,
                Password = password,
                Email = email
            };

            bool success = _financeRepository.CreateUser(user);
            if (success)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("User registered successfully! Please log in.");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Failed to register user.");
                Console.ResetColor();
            }
        }

       
        private bool Login()
        {
            Console.WriteLine("\nLogin to the Finance Management System");

            Console.Write("Enter Username: ");
            string username = Console.ReadLine();
            Console.Write("Enter Password: ");
            string password = Console.ReadLine();

            User user = _financeRepository.GetUserByUsername(username);
            if (user != null && user.Password == password)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Login successful!");
                DisplayUserDetails(user);  
                Console.ResetColor();
                return true;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid username or password.");
                Console.ResetColor();
                return false;
            }
        }

       
        private void DisplayUserDetails(User user)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            var table = new ConsoleTable("User ID", "Username", "Email");
            table.AddRow(user.UserId, user.Username, user.Email);
            table.Write();
            Console.ResetColor();
        }

        private void ShowMainMenu()
        {
            bool exit = false;

            while (!exit)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\nFinance Management System - Main Menu");
                Console.ResetColor();
                Console.WriteLine("1. Add Expense");
                Console.WriteLine("2. View All Expenses");
                Console.WriteLine("3. Generate Expense Report");
                Console.WriteLine("4. Update Expense");  
                Console.WriteLine("5. Delete User");
                Console.WriteLine("6. Delete Expense");
                Console.WriteLine("7. Log Out");
                Console.Write("Choose an option: ");

                if (!int.TryParse(Console.ReadLine(), out int option))
                {
                    Console.WriteLine("Invalid input! Please enter a valid number.");
                    continue;
                }

                switch (option)
                {
                    case 1:
                        AddExpense();
                        break;
                    case 2:
                        ViewAllExpenses();
                        break;
                    case 3:
                        GenerateExpenseReport();
                        break;
                    case 4:  
                        UpdateExpense();
                        break;
                    case 5:
                        DeleteUser();
                        break;
                    case 6:
                        DeleteExpense();
                        break;
                    case 7:
                        exit = true;
                        Console.WriteLine("Logging out...");
                        break;
                    default:
                        Console.WriteLine("Invalid option! Please choose a valid menu option.");
                        break;
                }
            }
        }


        
        private void GenerateExpenseReport()
        {
            Console.WriteLine("Generate Expense Report");
            Console.WriteLine("1. By User ID");
            Console.WriteLine("2. By Date Range");
            Console.Write("Choose an option: ");

            if (int.TryParse(Console.ReadLine(), out int reportOption))
            {
                switch (reportOption)
                {
                    case 1:
                        
                        Console.Write("Enter User ID: ");
                        if (int.TryParse(Console.ReadLine(), out int userId))
                        {
                            List<Expense> userExpenses = _financeRepository.GetAllExpenses(userId);
                            DisplayExpenses(userExpenses);
                        }
                        else
                        {
                            Console.WriteLine("Invalid User ID. Please enter a valid number.");
                        }
                        break;

                    case 2:
                       
                        Console.Write("Enter User ID: ");
                        if (int.TryParse(Console.ReadLine(), out userId))
                        {
                            
                            Console.Write("Enter Start Date (yyyy-MM-dd): ");
                            if (DateTime.TryParse(Console.ReadLine(), out DateTime startDate))
                            {
                                Console.Write("Enter End Date (yyyy-MM-dd): ");
                                if (DateTime.TryParse(Console.ReadLine(), out DateTime endDate))
                                {
                                    List<Expense> dateRangeExpenses = _financeRepository.GetExpensesByDateRange(userId, startDate, endDate);
                                    DisplayExpenses(dateRangeExpenses);
                                }
                                else
                                {
                                    Console.WriteLine("Invalid End Date. Please enter a valid date.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Invalid Start Date. Please enter a valid date.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid User ID. Please enter a valid number.");
                        }
                        break;

                    default:
                        Console.WriteLine("Invalid option selected.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Invalid input! Please enter a valid number.");
            }
        }


       
        private void DisplayExpenses(List<Expense> expenses)
        {
            if (expenses.Count > 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                var table = new ConsoleTable("Expense ID", "Amount", "Category ID", "Date", "Description");

                decimal totalAmount = 0;  
                foreach (var expense in expenses)
                {
                    table.AddRow(expense.ExpenseId, expense.Amount, expense.CategoryId, expense.Date.ToShortDateString(), expense.Description);
                    totalAmount += expense.Amount;  
                }

               
                table.Write();
                Console.ResetColor();

                
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                var summaryTable = new ConsoleTable("Total Number of Expenses", "Total Amount Spent");
                summaryTable.AddRow(expenses.Count, totalAmount);
                summaryTable.Write();
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine("No expenses found for the specified criteria.");
            }
        }


       
        private void AddExpense()
        {
            try
            {
                Console.Write("Enter User ID: ");
                int userId = int.Parse(Console.ReadLine());

                ShowExpenseCategories();

                Console.Write("Enter Category ID (from the table above): ");
                int categoryId = int.Parse(Console.ReadLine());

                Console.Write("Enter Amount: ");
                decimal amount = decimal.Parse(Console.ReadLine());
                Console.Write("Enter Date (yyyy-MM-dd): ");
                DateTime date = DateTime.Parse(Console.ReadLine());
                Console.Write("Enter Description: ");
                string description = Console.ReadLine();

                Expense expense = new Expense
                {
                    UserId = userId,
                    Amount = amount,
                    CategoryId = categoryId,
                    Date = date,
                    Description = description
                };

                bool success = _financeRepository.CreateExpense(expense);
                Console.WriteLine(success ? "Expense added successfully." : "Failed to add expense.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while adding the expense: {ex.Message}");
            }
        }

        
        private void ShowExpenseCategories()
        {
            List<ExpenseCategory> categories = _financeRepository.GetAllExpenseCategories();
            if (categories.Count > 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                var table = new ConsoleTable("Category ID", "Category Name");
                foreach (var category in categories)
                {
                    table.AddRow(category.CategoryId, category.CategoryName);
                }
                table.Write();
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine("No expense categories available.");
            }
        }

       
        private void DeleteUser()
        {
            try
            {
                ShowAllUsers();  

                Console.Write("Enter User ID to delete: ");
                int userId = int.Parse(Console.ReadLine());

                if (!_financeRepository.UserExists(userId))
                {
                    throw new UserNotFoundException($"User with ID {userId} not found.");
                }

                bool success = _financeRepository.DeleteUser(userId);
                Console.WriteLine(success ? "User deleted successfully." : "Failed to delete user.");
            }
            catch (UserNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

       
        private void ShowAllUsers()
        {
            List<User> users = _financeRepository.GetAllUsers();
            if (users.Count > 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                var table = new ConsoleTable("User ID", "Username", "Email");
                foreach (var user in users)
                {
                    table.AddRow(user.UserId, user.Username, user.Email);
                }
                table.Write();
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine("No users found.");
            }
        }

       
        private void DeleteExpense()
        {
            try
            {
                ViewAllExpenses();  

                Console.Write("Enter Expense ID to delete: ");
                int expenseId = int.Parse(Console.ReadLine());

                bool success = _financeRepository.DeleteExpense(expenseId);
                Console.WriteLine(success ? "Expense deleted successfully." : "Failed to delete expense.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while deleting the expense: {ex.Message}");
            }
        }

       
        private void UpdateExpense()
        {
            try
            {
                ViewAllExpenses();  

                Console.Write("Enter Expense ID: ");
                if (!int.TryParse(Console.ReadLine(), out int expenseId))
                {
                    Console.WriteLine("Invalid Expense ID. Please enter a numeric value.");
                    return;
                }

                
                if (!_financeRepository.ExpenseExists(expenseId))
                {
                    throw new ExpenseNotFoundException($"Expense with ID {expenseId} not found.");
                }

                
                Expense currentExpense = _financeRepository.GetExpenseById(expenseId); 

                
                Console.WriteLine("Current Expense Details:");
                Console.WriteLine($"Amount: {currentExpense.Amount}");
                Console.WriteLine($"Category ID: {currentExpense.CategoryId}");
                Console.WriteLine($"Date: {currentExpense.Date.ToShortDateString()}");
                Console.WriteLine($"Description: {currentExpense.Description}");

               
                Console.Write("Update Amount? (Y/N): ");
                if (Console.ReadLine().Trim().ToUpper() == "Y")
                {
                    Console.Write("Enter New Amount: ");
                    if (decimal.TryParse(Console.ReadLine(), out decimal newAmount))
                    {
                        currentExpense.Amount = newAmount; 
                    }
                    else
                    {
                        Console.WriteLine("Invalid amount. Keeping the previous value.");
                    }
                }

                Console.Write("Update Category ID? (Y/N): ");
                if (Console.ReadLine().Trim().ToUpper() == "Y")
                {
                    Console.Write("Enter New Category ID: ");
                    if (int.TryParse(Console.ReadLine(), out int newCategoryId))
                    {
                        currentExpense.CategoryId = newCategoryId;
                    }
                    else
                    {
                        Console.WriteLine("Invalid category ID. Keeping the previous value.");
                    }
                }

                Console.Write("Update Date? (Y/N): ");
                if (Console.ReadLine().Trim().ToUpper() == "Y")
                {
                    Console.Write("Enter New Date (yyyy-MM-dd): ");
                    if (DateTime.TryParse(Console.ReadLine(), out DateTime newDate))
                    {
                        
                        if (newDate >= new DateTime(1753, 1, 1) && newDate <= new DateTime(9999, 12, 31))
                        {
                            currentExpense.Date = newDate; 
                        }
                        else
                        {
                            Console.WriteLine("Date must be between 1/1/1753 and 12/31/9999. Keeping the previous value.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid date format. Keeping the previous value.");
                    }
                }

                Console.Write("Update Description? (Y/N): ");
                if (Console.ReadLine().Trim().ToUpper() == "Y")
                {
                    Console.Write("Enter New Description: ");
                    string newDescription = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(newDescription))
                    {
                        currentExpense.Description = newDescription; 
                    }
                    else
                    {
                        Console.WriteLine("Description cannot be empty. Keeping the previous value.");
                    }
                }

               
                bool success = _financeRepository.UpdateExpense(expenseId, currentExpense);
                Console.WriteLine(success ? "Expense updated successfully." : "Failed to update expense.");
            }
            catch (ExpenseNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while updating the expense: {ex.Message}");
            }
        }


        
        private void ViewAllExpenses()
        {
            try
            {
                Console.Write("Enter User ID: ");
                int userId = int.Parse(Console.ReadLine());

                List<Expense> expenses = _financeRepository.GetAllExpenses(userId);
                if (expenses.Count > 0)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    var table = new ConsoleTable("Expense ID", "Amount", "Category ID", "Date", "Description");
                    foreach (var expense in expenses)
                    {
                        table.AddRow(expense.ExpenseId, expense.Amount, expense.CategoryId, expense.Date.ToShortDateString(), expense.Description);
                    }
                    table.Write();
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine("No expenses found for the user.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving expenses: {ex.Message}");
            }
        }
    }
}
