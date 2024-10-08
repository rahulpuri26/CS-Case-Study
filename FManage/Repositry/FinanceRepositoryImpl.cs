using System.Data.SqlClient;
using FManage.Exceptions;
using FManage.Model;
using FManage.Repositry.interfaces;
using FManage.Utilities;

namespace FManage.Repositry
{
    public class FinanceRepositoryImpl : IFinanceRepository
    {
        
        private SqlConnection sqlConnection;
        private SqlCommand cmd;

       
        public FinanceRepositoryImpl()
        {
            sqlConnection = new SqlConnection(DbConnUtil.GetConnString()); 
            cmd = new SqlCommand();
            cmd.Connection = sqlConnection;
        }

        private void ClearCommandParameters()
        {
            cmd.Parameters.Clear();
        }

        public User GetUserByUsername(string username)
        {
            User user = null;

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(DbConnUtil.GetConnString()))
                {
                    sqlConnection.Open();

                    string query = "SELECT user_id, username, password, email FROM users WHERE username = @username";
                    using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
                    {
                        cmd.Parameters.AddWithValue("@username", username);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                user = new User
                                {
                                    UserId = (int)reader["user_id"],
                                    Username = (string)reader["username"],
                                    Password = (string)reader["password"],
                                    Email = (string)reader["email"]
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving the user: {ex.Message}");
            }

            return user;
        }

        public List<User> GetAllUsers()
        {
            List<User> users = new List<User>();

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(DbConnUtil.GetConnString()))
                {
                    sqlConnection.Open();

                    string query = "SELECT user_id, username, email FROM users";
                    using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                User user = new User
                                {
                                    UserId = (int)reader["user_id"],
                                    Username = (string)reader["username"],
                                    Email = (string)reader["email"]
                                };
                                users.Add(user);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving users: {ex.Message}");
            }

            return users;
        }
        
        public bool UserExists(int userId)
        {
            cmd.CommandText = "SELECT COUNT(*) FROM users WHERE user_id = @UserId";
            cmd.Parameters.AddWithValue("@UserId", userId);

            sqlConnection.Open();
            int count = (int)cmd.ExecuteScalar();
            sqlConnection.Close();

            ClearCommandParameters();
            return count > 0;
        }

        
        public bool ExpenseExists(int expenseId)
        {
            cmd.CommandText = "SELECT COUNT(*) FROM expenses WHERE expense_id = @ExpenseId";
            cmd.Parameters.AddWithValue("@ExpenseId", expenseId);

            sqlConnection.Open();
            int count = (int)cmd.ExecuteScalar();
            sqlConnection.Close();

            ClearCommandParameters();
            return count > 0;
        }



        
        public bool CreateUser(User user)
        {
            bool isSuccess = false;

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(DbConnUtil.GetConnString()))
                {
                    sqlConnection.Open();

                    
                    string query = "INSERT INTO users (username, password, email) VALUES (@username, @password, @email)";
                    using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
                    {
                        cmd.Parameters.AddWithValue("@username", user.Username);
                        cmd.Parameters.AddWithValue("@password", user.Password);
                        cmd.Parameters.AddWithValue("@email", user.Email);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        isSuccess = rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while adding the user: {ex.Message}");
            }

            return isSuccess;
        }


        
        public bool CreateExpense(Expense expense)
        {
            bool isSuccess = false;

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(DbConnUtil.GetConnString()))
                {
                    sqlConnection.Open();

                    
                    string query = "INSERT INTO expenses (user_id, amount, category_id, date, description) VALUES (@userId, @amount, @categoryId, @date, @description)";
                    using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
                    {
                        cmd.Parameters.AddWithValue("@userId", expense.UserId);
                        cmd.Parameters.AddWithValue("@amount", expense.Amount);
                        cmd.Parameters.AddWithValue("@categoryId", expense.CategoryId);
                        cmd.Parameters.AddWithValue("@date", expense.Date);
                        cmd.Parameters.AddWithValue("@description", expense.Description);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        isSuccess = rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while adding the expense: {ex.Message}");
            }

            return isSuccess;
        }
        public List<Expense> GetExpensesByDateRange(int userId, DateTime startDate, DateTime endDate)
        {
            List<Expense> expenses = new List<Expense>();

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(DbConnUtil.GetConnString()))
                {
                    sqlConnection.Open();
                    string query = "SELECT * FROM expenses WHERE user_id = @UserId AND date BETWEEN @StartDate AND @EndDate";
                    using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
                    {
                        cmd.Parameters.AddWithValue("@UserId", userId);
                        cmd.Parameters.AddWithValue("@StartDate", startDate);
                        cmd.Parameters.AddWithValue("@EndDate", endDate);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Expense expense = new Expense
                                {
                                    ExpenseId = (int)reader["expense_id"],
                                    UserId = (int)reader["user_id"],
                                    Amount = (decimal)reader["amount"],
                                    CategoryId = (int)reader["category_id"],
                                    Date = (DateTime)reader["date"],
                                    Description = reader["description"].ToString()
                                };
                                expenses.Add(expense);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving expenses: {ex.Message}");
            }

            return expenses;
        }



        
        public bool DeleteUser(int userId)
        {
            try
            {
                if (!UserExists(userId))
                {
                    throw new UserNotFoundException($"User with ID {userId} not found.");
                }

                cmd.CommandText = "DELETE FROM users WHERE user_id = @UserId";
                cmd.Parameters.AddWithValue("@UserId", userId);

                sqlConnection.Open();
                int result = cmd.ExecuteNonQuery();
                sqlConnection.Close();

                ClearCommandParameters();
                return result > 0;
            }
            catch (UserNotFoundException)
            {
                throw;  
            }
            catch (Exception)
            {
                sqlConnection.Close();
                ClearCommandParameters();
                return false;
            }
        }

        
        public bool DeleteExpense(int expenseId)
        {
            try
            {
                if (!ExpenseExists(expenseId))
                {
                    throw new ExpenseNotFoundException($"Expense with ID {expenseId} not found.");
                }

                cmd.CommandText = "DELETE FROM expenses WHERE expense_id = @ExpenseId";
                cmd.Parameters.AddWithValue("@ExpenseId", expenseId);

                sqlConnection.Open();
                int result = cmd.ExecuteNonQuery();
                sqlConnection.Close();

                ClearCommandParameters();
                return result > 0;
            }
            catch (ExpenseNotFoundException)
            {
                throw;  
            }
            catch (Exception)
            {
                sqlConnection.Close();
                ClearCommandParameters();
                return false;
            }
        }

        public Expense GetExpenseById(int expenseId)
        {
            using (var connection = new SqlConnection(DbConnUtil.GetConnString()))
            {
                connection.Open();
                string query = "SELECT * FROM expenses WHERE expense_id = @ExpenseId";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ExpenseId", expenseId);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Expense
                            {
                                ExpenseId = (int)reader["expense_id"],
                                Amount = (decimal)reader["amount"],
                                CategoryId = (int)reader["category_id"],
                                Date = (DateTime)reader["date"],
                                Description = (string)reader["description"]
                            };
                        }
                    }
                }
            }
            throw new ExpenseNotFoundException($"Expense with ID {expenseId} not found.");
        }


        
        public List<Expense> GetAllExpenses(int userId)
        {
            List<Expense> expenses = new List<Expense>();

            
            string query = "SELECT expense_id, user_id, amount, category_id, date, description FROM expenses WHERE user_id = @UserId";

            
            using (SqlConnection connection = new SqlConnection(DbConnUtil.GetConnString()))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserId", userId);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                
                while (reader.Read())
                {
                    Expense expense = new Expense
                    {
                        ExpenseId = reader.GetInt32(0),
                        UserId = reader.GetInt32(1),
                        Amount = reader.GetDecimal(2),  
                        CategoryId = reader.GetInt32(3),
                        Date = reader.GetDateTime(4),
                        Description = reader.GetString(5)
                    };
                    expenses.Add(expense);
                }
            }

            return expenses;
        }


        public List<ExpenseCategory> GetAllExpenseCategories()
        {
            List<ExpenseCategory> categories = new List<ExpenseCategory>();

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(DbConnUtil.GetConnString()))
                {
                    sqlConnection.Open();

                    string query = "SELECT category_id, category_name FROM expensecategories";
                    using (SqlCommand cmd = new SqlCommand(query, sqlConnection))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                categories.Add(new ExpenseCategory
                                {
                                    CategoryId = (int)reader["category_id"],
                                    CategoryName = (string)reader["category_name"]
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving categories: {ex.Message}");
            }

            return categories;
        }


        
        public bool UpdateExpense(int expenseId, Expense expense)
        {
            try
            {
                
                if (!ExpenseExists(expenseId))
                {
                    throw new ExpenseNotFoundException($"Expense with ID {expenseId} not found.");
                }

                string query = "UPDATE expenses SET amount = @Amount, category_id = @CategoryId, date = @Date, description = @Description WHERE expense_id = @ExpenseId";

                using (SqlConnection sqlConnection = new SqlConnection(DbConnUtil.GetConnString()))
                {
                    SqlCommand cmd = new SqlCommand(query, sqlConnection);
                    cmd.Parameters.AddWithValue("@Amount", expense.Amount);
                    cmd.Parameters.AddWithValue("@CategoryId", expense.CategoryId);
                    cmd.Parameters.AddWithValue("@Date", expense.Date);
                    cmd.Parameters.AddWithValue("@Description", expense.Description);
                    cmd.Parameters.AddWithValue("@ExpenseId", expense.ExpenseId); 

                    sqlConnection.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    return rowsAffected > 0;  
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while updating the expense: {ex.Message}");
                return false;
            }
        }

    }
}
