using System;
using System.Collections.Generic;

namespace MarketProgram
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public List<CartItem> Cart { get; private set; }
        public List<string> PurchaseHistory { get; private set; }

        public User(string username, string password, string name, string email)
        {
            Username = username;
            Password = password;
            Name = name;
            Email = email;
            Cart = new List<CartItem>();
            PurchaseHistory = new List<string>();
        }

        public void UpdateProfile(string name, string email)
        {
            Name = name;
            Email = email;
        }

        public void ChangePassword(string newPassword)
        {
            Password = newPassword;
        }
    }
}
