using System;
using System.Collections.Generic;

namespace MarketProgram
{
    public class Menu
    {
        private Store _store;

        public Menu(Store store)
        {
            _store = store;
        }

        public void MainMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Market Programına Hoşgeldiniz");
                Console.WriteLine("1. Kullanıcı Girişi");
                Console.WriteLine("2. Kullanıcı Kaydı");
                Console.WriteLine("3. Admin Girişi");
                Console.WriteLine("4. Çıkış");
                Console.Write("Seçiminiz: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        UserLogin();
                        break;
                    case "2":
                        UserRegister();
                        break;
                    case "3":
                        AdminLogin();
                        break;
                    case "4":
                        return;
                    default:
                        Console.WriteLine("Geçersiz seçim. Tekrar deneyin.");
                        break;
                }
            }
        }

        private void UserLogin()
        {
            Console.Clear();
            Console.Write("Kullanıcı Adı: ");
            string username = Console.ReadLine();
            Console.Write("Şifre: ");
            string password = Console.ReadLine();

            User user = _store.LoginUser(username, password);

            if (user != null)
            {
                UserPanel(user);
            }
            else
            {
                Console.WriteLine("Kullanıcı adı veya şifre yanlış.");
                Console.ReadLine();
            }
        }

        private void UserRegister()
        {
            Console.Clear();
            Console.Write("Kullanıcı Adı: ");
            string username = Console.ReadLine();
            Console.Write("Şifre: ");
            string password = Console.ReadLine();
            Console.Write("Ad: ");
            string name = Console.ReadLine();
            Console.Write("Email: ");
            string email = Console.ReadLine();

            _store.RegisterUser(username, password, name, email);
            Console.WriteLine("Kayıt başarılı.");
            Console.ReadLine();
        }

        private void AdminLogin()
        {
            Console.Clear();
            Console.Write("Admin Kullanıcı Adı: ");
            string username = Console.ReadLine();
            Console.Write("Admin Şifre: ");
            string password = Console.ReadLine();

            Admin admin = _store.LoginAdmin(username, password);

            if (admin != null)
            {
                AdminPanel(admin);
            }
            else
            {
                Console.WriteLine("Admin kullanıcı adı veya şifre yanlış.");
                Console.ReadLine();
            }
        }

        private void UserPanel(User user)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"Hoşgeldiniz, {user.Name}");
                Console.WriteLine("1. Kategorileri Görüntüle");
                Console.WriteLine("2. Sepete Bak");
                Console.WriteLine("3. Profil");
                Console.WriteLine("4. Çıkış");
                Console.Write("Seçiminiz: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        ViewCategories(user);
                        break;
                    case "2":
                        ViewCart(user);
                        break;
                    case "3":
                        ViewProfile(user);
                        break;
                    case "4":
                        return;
                    default:
                        Console.WriteLine("Geçersiz seçim. Tekrar deneyin.");
                        break;
                }
            }
        }

        private void ViewCategories(User user)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Kategoriler:");
                for (int i = 0; i < _store.Categories.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {_store.Categories[i].Name}");
                }
                Console.WriteLine("0. Geri");
                Console.Write("Seçiminiz: ");

                if (int.TryParse(Console.ReadLine(), out int choice) && choice >= 0 && choice <= _store.Categories.Count)
                {
                    if (choice == 0)
                    {
                        return;
                    }
                    ViewProductsByCategory(user, _store.Categories[choice - 1].Name);
                }
                else
                {
                    Console.WriteLine("Geçersiz seçim. Tekrar deneyin.");
                }
            }
        }

        private void ViewProductsByCategory(User user, string category)
        {
            while (true)
            {
                Console.Clear();
                List<Product> products = _store.GetProductsByCategory(category);
                if (products.Count == 0)
                {
                    Console.WriteLine("Bu kategoride ürün bulunmamaktadır.");
                    Console.ReadLine();
                    return;
                }

                Console.WriteLine($"{category} Kategorisindeki Ürünler:");
                for (int i = 0; i < products.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {products[i].Name} - {products[i].Price:C} - Stok: {products[i].Stock}");
                }
                Console.WriteLine("0. Geri");
                Console.Write("Sepete eklemek istediğiniz ürünü seçin: ");

                if (int.TryParse(Console.ReadLine(), out int choice) && choice >= 0 && choice <= products.Count)
                {
                    if (choice == 0)
                    {
                        return;
                    }
                    AddToCart(user, products[choice - 1]);
                }
                else
                {
                    Console.WriteLine("Geçersiz seçim. Tekrar deneyin.");
                }
            }
        }

        private void AddToCart(User user, Product product)
        {
            Console.Write("Kaç adet eklemek istiyorsunuz: ");
            if (int.TryParse(Console.ReadLine(), out int quantity) && quantity > 0 && quantity <= product.Stock)
            {
                user.Cart.Add(new CartItem(product, quantity));
                product.Stock -= quantity;
                Console.WriteLine("Ürün sepete eklendi.");
            }
            else
            {
                Console.WriteLine("Geçersiz miktar.");
            }
            Console.ReadLine();
        }

        private void ViewCart(User user)
        {
            while (true)
            {
                Console.Clear();
                if (user.Cart.Count == 0)
                {
                    Console.WriteLine("Sepetiniz boş.");
                    Console.ReadLine();
                    return;
                }

                double total = 0;
                Console.WriteLine("Sepetiniz:");
                for (int i = 0; i < user.Cart.Count; i++)
                {
                    var item = user.Cart[i];
                    total += item.Product.Price * item.Quantity;
                    Console.WriteLine($"{i + 1}. {item.Product.Name} - {item.Quantity} adet - {item.Product.Price * item.Quantity:C}");
                }
                Console.WriteLine($"Toplam: {total:C}");
                Console.WriteLine("1. Ödeme Yap");
                Console.WriteLine("2. Sepetten Ürün Çıkar");
                Console.WriteLine("0. Geri");
                Console.Write("Seçiminiz: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        Checkout(user, total);
                        return;
                    case "2":
                        RemoveFromCart(user);
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Geçersiz seçim. Tekrar deneyin.");
                        break;
                }
            }
        }

        private void RemoveFromCart(User user)
        {
            Console.Write("Çıkarmak istediğiniz ürünü seçin: ");
            if (int.TryParse(Console.ReadLine(), out int choice) && choice > 0 && choice <= user.Cart.Count)
            {
                var item = user.Cart[choice - 1];
                item.Product.Stock += item.Quantity;
                user.Cart.RemoveAt(choice - 1);
                Console.WriteLine("Ürün sepetten çıkarıldı.");
            }
            else
            {
                Console.WriteLine("Geçersiz seçim.");
            }
            Console.ReadLine();
        }

        private void Checkout(User user, double total)
        {
            Console.Write("Ödemek istediğiniz miktarı girin: ");
            if (double.TryParse(Console.ReadLine(), out double payment) && payment >= total)
            {
                double change = payment - total;
                user.PurchaseHistory.Add($"Toplam: {total:C} - Ödeme: {payment:C} - Para Üstü: {change:C}");
                user.Cart.Clear();
                Console.WriteLine($"Ödeme başarılı. Para üstü: {change:C}");
            }
            else
            {
                Console.WriteLine("Yetersiz miktar.");
            }
            Console.ReadLine();
        }

        private void ViewProfile(User user)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Profiliniz:");
                Console.WriteLine($"Ad: {user.Name}");
                Console.WriteLine($"Email: {user.Email}");
                Console.WriteLine("1. Bilgileri Güncelle");
                Console.WriteLine("2. Şifre Değiştir");
                Console.WriteLine("3. Alışveriş Geçmişi");
                Console.WriteLine("0. Geri");
                Console.Write("Seçiminiz: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        UpdateProfile(user);
                        break;
                    case "2":
                        ChangePassword(user);
                        break;
                    case "3":
                        ViewPurchaseHistory(user);
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Geçersiz seçim. Tekrar deneyin.");
                        break;
                }
            }
        }

        private void UpdateProfile(User user)
        {
            Console.Write("Yeni Ad: ");
            string name = Console.ReadLine();
            Console.Write("Yeni Email: ");
            string email = Console.ReadLine();
            user.UpdateProfile(name, email);
            Console.WriteLine("Profil güncellendi.");
            Console.ReadLine();
        }

        private void ChangePassword(User user)
        {
            Console.Write("Yeni Şifre: ");
            string password = Console.ReadLine();
            user.ChangePassword(password);
            Console.WriteLine("Şifre değiştirildi.");
            Console.ReadLine();
        }

        private void ViewPurchaseHistory(User user)
        {
            Console.Clear();
            Console.WriteLine("Alışveriş Geçmişiniz:");
            foreach (var history in user.PurchaseHistory)
            {
                Console.WriteLine(history);
            }
            Console.ReadLine();
        }

        private void AdminPanel(Admin admin)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"Hoşgeldiniz, {admin.Username}");
                Console.WriteLine("1. Stok Yönetimi");
                Console.WriteLine("2. Kategorileri Yönet");
                Console.WriteLine("3. Raporlar");
                Console.WriteLine("4. Çıkış");
                Console.Write("Seçiminiz: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        ManageStock();
                        break;
                    case "2":
                        ManageCategories();
                        break;
                    case "3":
                        ViewReports();
                        break;
                    case "4":
                        return;
                    default:
                        Console.WriteLine("Geçersiz seçim. Tekrar deneyin.");
                        break;
                }
            }
        }

        private void ManageStock()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Stok Yönetimi:");
                for (int i = 0; i < _store.Products.Count; i++)
                {
                    var product = _store.Products[i];
                    Console.WriteLine($"{i + 1}. {product.Name} - {product.Category} - {product.Price:C} - Stok: {product.Stock}");
                }
                Console.WriteLine("1. Ürün Ekle");
                Console.WriteLine("2. Ürün Stok Güncelle");
                Console.WriteLine("0. Geri");
                Console.Write("Seçiminiz: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        AddProduct();
                        break;
                    case "2":
                        UpdateProductStock();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Geçersiz seçim. Tekrar deneyin.");
                        break;
                }
            }
        }

        private void AddProduct()
        {
            Console.Write("Ürün Adı: ");
            string name = Console.ReadLine();
            Console.Write("Kategori: ");
            string category = Console.ReadLine();
            Console.Write("Fiyat: ");
            if (double.TryParse(Console.ReadLine(), out double price))
            {
                Console.Write("Stok: ");
                if (int.TryParse(Console.ReadLine(), out int stock))
                {
                    _store.AddProduct(name, category, price, stock);
                    Console.WriteLine("Ürün eklendi.");
                }
                else
                {
                    Console.WriteLine("Geçersiz stok.");
                }
            }
            else
            {
                Console.WriteLine("Geçersiz fiyat.");
            }
            Console.ReadLine();
        }

        private void UpdateProductStock()
        {
            Console.Write("Stok güncellemek istediğiniz ürünü seçin: ");
            if (int.TryParse(Console.ReadLine(), out int choice) && choice > 0 && choice <= _store.Products.Count)
            {
                var product = _store.Products[choice - 1];
                Console.Write($"Yeni stok miktarını girin ({product.Stock}): ");
                if (int.TryParse(Console.ReadLine(), out int newStock))
                {
                    _store.UpdateProductStock(product.Name, newStock);
                    Console.WriteLine("Stok güncellendi.");
                }
                else
                {
                    Console.WriteLine("Geçersiz miktar.");
                }
            }
            else
            {
                Console.WriteLine("Geçersiz seçim.");
            }
            Console.ReadLine();
        }

        private void ManageCategories()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Kategoriler:");
                for (int i = 0; i < _store.Categories.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {_store.Categories[i].Name}");
                }
                Console.WriteLine("1. Kategori Ekle");
                Console.WriteLine("0. Geri");
                Console.Write("Seçiminiz: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        AddCategory();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Geçersiz seçim. Tekrar deneyin.");
                        break;
                }
            }
        }

        private void AddCategory()
        {
            Console.Write("Kategori Adı: ");
            string name = Console.ReadLine();
            _store.AddCategory(name);
            Console.WriteLine("Kategori eklendi.");
            Console.ReadLine();
        }

        private void ViewReports()
        {
            Console.Clear();
            Console.WriteLine("Raporlar:");
            // Buraya raporları ekleyebilirsiniz
            Console.ReadLine();
        }
    }
}
