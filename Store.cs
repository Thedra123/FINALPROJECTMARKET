using System;
using System.Collections.Generic;
using System.Linq;

namespace MarketProgram
{
    public class Store
    {
        public List<User> Users { get; private set; }
        public List<Admin> Admins { get; private set; }
        public List<Product> Products { get; private set; }
        public List<Category> Categories { get; private set; }

        public Store()
        {
            Users = new List<User>();
            Admins = new List<Admin>();
            Products = new List<Product>();
            Categories = new List<Category>();

            // Örnek veri ekleyelim
            Admins.Add(new Admin("admin", "admin123"));
            Categories.Add(new Category("Electronics"));
            Products.Add(new Product("Laptop", "Electronics", 1500.00, 10));
        }

        public User LoginUser(string username, string password)
        {
            return Users.FirstOrDefault(u => u.Username == username && u.Password == password);
        }

        public Admin LoginAdmin(string username, string password)
        {
            return Admins.FirstOrDefault(a => a.Username == username && a.Password == password);
        }

        public void RegisterUser(string username, string password, string name, string email)
        {
            Users.Add(new User(username, password, name, email));
        }

        public void AddProduct(string name, string category, double price, int stock)
        {
            Products.Add(new Product(name, category, price, stock));
        }

        public void UpdateProductStock(string productName, int newStock)
        {
            var product = Products.FirstOrDefault(p => p.Name == productName);
            if (product != null)
            {
                product.UpdateStock(newStock);
            }
        }

        public void AddCategory(string categoryName)
        {
            Categories.Add(new Category(categoryName));
        }

        public List<Product> GetProductsByCategory(string category)
        {
            return Products.Where(p => p.Category == category && p.Stock > 0).ToList();
        }
    }
}
