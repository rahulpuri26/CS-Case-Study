using System;
namespace FManage.Model
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

        public User() { }

        public User(int userId, string username, string password, string email)
        {
            UserId = userId;
            Username = username;
            Password = password;
            Email = email;
        }
    }
}
