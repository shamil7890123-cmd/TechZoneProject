using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TechZoneProject.Models;
using System.Data.Entity;
using System.Threading.Tasks; 

namespace TechZoneProject.Controllers
{
    public class HomeController : Controller
    {
        // --- ГЛАВНАЯ СТРАНИЦА И ИНИЦИАЛИЗАЦИЯ ---
        [OutputCache(Duration = 60, VaryByParam = "none")] 
        public async Task<ActionResult> Index()
        {
            using (TechZoneContext db = new TechZoneContext())
            {
                if (!await db.Products.AnyAsync())
                {
                    var sampleProducts = new List<Product>
                    {
                        new Product {
                            Name = "Игровая мышь PRO", Price = 14990, Category = "Периферия", ImageUrl = "mouse.jpg",
                            Processor = "Hero 25K Sensor", VideoCard = "Lightspeed Wireless", RAM = "5 профилей", Storage = "—",
                            Description = "Сверхлегкая конструкция в сочетании с легендарным сенсором HERO 25K обеспечивает феноменальную точность.",
                            Stock = 10
                        },
                        new Product {
                            Name = "Механика RGB", Price = 18500, Category = "Периферия", ImageUrl = "keyboard.jpg",
                            Processor = "ARM Cortex-M4", VideoCard = "RGB Controller", RAM = "Onboard Flash", Storage = "—",
                            Description = "Премиальная механическая клавиатура с переключателями Tactile Brown.",
                            Stock = 7
                        },
                        new Product {
                            Name = "Игровой ноутбук", Price = 92200, Category = "Ноутбуки", ImageUrl = "laptop.jpg",
                            Processor = "Intel Core i5-12500H", VideoCard = "NVIDIA RTX 3060", RAM = "16 ГБ DDR4", Storage = "512 ГБ SSD",
                            Description = "Мощная игровая станция в компактном корпусе. Экран 144 Гц.",
                            Stock = 5
                        },
                        new Product {
                            Name = "Смартфон Ultra", Price = 62900, Category = "Телефоны", ImageUrl = "smartphone.jpg",
                            Processor = "Snapdragon 8 Gen 2", VideoCard = "Adreno 740", RAM = "12 ГБ LPDDR5X", Storage = "256 ГБ UFS 4.0",
                            Description = "Инновационный дисплей Dynamic AMOLED 2X. Революционная камера на 200 Мп.",
                            Stock = 12
                        },
                        new Product {
                            Name = "MSI Stealth Gaming", Price = 115000, Category = "Ноутбуки", ImageUrl = "laptop-msi.png",
                            Processor = "Intel Core i7-13700H", VideoCard = "NVIDIA RTX 4070", RAM = "16 ГБ DDR5", Storage = "1 ТБ SSD",
                            Description = "Воплощение стиля и мощи. Толщина всего 19 мм.",
                            Stock = 3
                        },
                        new Product {
                            Name = "Игровой ПК ASUS ROG", Price = 189990, Category = "ПК", ImageUrl = "pc-asus.png",
                            Processor = "Intel Core i9-13900K", VideoCard = "NVIDIA RTX 4090", RAM = "64 ГБ DDR5", Storage = "2 ТБ SSD",
                            Description = "Бескомпромиссная производительность для 4K-гейминга.",
                            Stock = 2
                        }
                    };
                    db.Products.AddRange(sampleProducts);
                    await db.SaveChangesAsync();
                }
                return View(await db.Products.ToListAsync());
            }
        }

        // --- БЛОК АВТОРИЗАЦИИ ---
        public ActionResult Login() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(string Login, string Password)
        {
            using (TechZoneContext db = new TechZoneContext())
            {
                var user = await db.Users.FirstOrDefaultAsync(u => u.Login == Login && u.Password == Password);
                if (user != null)
                {
                    Session["UserId"] = user.Id;
                    Session["UserRole"] = user.Role;
                    Session["UserName"] = user.Login;
                    return RedirectToAction("Account");
                }
                ViewBag.Error = "Неверный логин или пароль";
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(string Login, string Password, string Email)
        {
            if (string.IsNullOrEmpty(Login) || string.IsNullOrEmpty(Password)) return RedirectToAction("Login");
            using (TechZoneContext db = new TechZoneContext())
            {
                if (await db.Users.AnyAsync(u => u.Login == Login)) return RedirectToAction("Login");
                db.Users.Add(new User { Login = Login, Password = Password, Email = Email, Role = "User" });
                await db.SaveChangesAsync();
                return RedirectToAction("Login");
            }
        }

        public ActionResult Logout() { Session.Clear(); return RedirectToAction("Index"); }

        // --- КАТАЛОГ И ТОВАРЫ ---
        [OutputCache(Duration = 30, VaryByParam = "category;minPrice;maxPrice;sortOrder")]
        public async Task<ActionResult> Catalog(string category, decimal? minPrice, decimal? maxPrice, string sortOrder)
        {
            using (TechZoneContext db = new TechZoneContext())
            {
                var products = db.Products.AsQueryable();
                if (!string.IsNullOrEmpty(category)) products = products.Where(p => p.Category == category);
                if (minPrice.HasValue) products = products.Where(p => p.Price >= minPrice.Value);
                if (maxPrice.HasValue) products = products.Where(p => p.Price <= maxPrice.Value);

                switch (sortOrder)
                {
                    case "price_desc": products = products.OrderByDescending(p => p.Price); break;
                    case "price": products = products.OrderBy(p => p.Price); break;
                    default: products = products.OrderBy(p => p.Name); break;
                }
                return View(await products.ToListAsync());
            }
        }

        public async Task<ActionResult> ProductDetails(int? id)
        {
            if (id == null) return RedirectToAction("Index");
            using (TechZoneContext db = new TechZoneContext())
            {
                var product = await db.Products.FindAsync(id);
                if (product == null) return HttpNotFound();
                return View(product);
            }
        }

        // --- КОРЗИНА И ОФОРМЛЕНИЕ ЗАКАЗА ---
        [HttpPost]
        public ActionResult AddToCart(int productId)
        {
            List<int> cart = Session["Cart"] as List<int> ?? new List<int>();
            cart.Add(productId);
            Session["Cart"] = cart;
            return RedirectToAction("Cart");
        }

        [HttpPost]
        public ActionResult RemoveOneFromCart(int productId)
        {
            List<int> cart = Session["Cart"] as List<int>;
            if (cart != null)
            {
                int index = cart.IndexOf(productId);
                if (index != -1)
                {
                    cart.RemoveAt(index);
                    Session["Cart"] = cart;
                }
            }
            return RedirectToAction("Cart");
        }

        public async Task<ActionResult> Cart()
        {
            using (TechZoneContext db = new TechZoneContext())
            {
                List<int> cartIds = Session["Cart"] as List<int> ?? new List<int>();
                var productCounts = cartIds.GroupBy(id => id).ToDictionary(g => g.Key, g => g.Count());
                var cartProducts = await db.Products.Where(p => productCounts.Keys.Contains(p.Id)).ToListAsync();
                ViewBag.ProductCounts = productCounts;

                if (Session["UserName"] != null)
                {
                    string currentName = Session["UserName"].ToString();
                    ViewBag.OrderHistory = await db.Orders.Where(o => o.CustomerName == currentName).OrderByDescending(o => o.OrderDate).ToListAsync();
                }
                return View(cartProducts);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Checkout(decimal totalSum)
        {
            if (Session["UserId"] == null) return RedirectToAction("Login");
            using (TechZoneContext db = new TechZoneContext())
            {
                List<int> cartIds = Session["Cart"] as List<int>;
                if (cartIds != null)
                {
                    foreach (var id in cartIds)
                    {
                        var product = await db.Products.FindAsync(id);
                        if (product != null && product.Stock > 0) product.Stock--;
                    }
                }

                var newOrder = new Order
                {
                    OrderNumber = new Random().Next(10000, 99999).ToString(),
                    OrderDate = DateTime.Now,
                    CustomerName = Session["UserName"]?.ToString(),
                    TotalAmount = totalSum,
                    Status = "Принят"
                };
                db.Orders.Add(newOrder);
                await db.SaveChangesAsync();
            }
            Session["Cart"] = null;
            TempData["Message"] = "Заказ успешно оформлен!";
            return RedirectToAction("Cart");
        }

        // --- ЛИЧНЫЙ КАБИНЕТ И АДМИНКА ---
        public async Task<ActionResult> Account()
        {
            if (Session["UserId"] == null) return RedirectToAction("Login");
            using (TechZoneContext db = new TechZoneContext())
            {
                int id = (int)Session["UserId"];
                var user = await db.Users.FindAsync(id);
                if (user == null) return RedirectToAction("Logout");
                ViewBag.UserOrders = await db.Orders.Where(o => o.CustomerName == user.Login).OrderByDescending(o => o.OrderDate).ToListAsync();
                return View(user);
            }
        }

        public ActionResult Dashboard() => Session["UserRole"]?.ToString() == "Admin" ? (ActionResult)View() : RedirectToAction("Login");

        public async Task<ActionResult> Orders(string searchTerm)
        {
            if (Session["UserRole"]?.ToString() != "Admin") return RedirectToAction("Login");
            using (TechZoneContext db = new TechZoneContext())
            {
                var ordersQuery = db.Orders.AsQueryable();
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    ViewBag.CurrentFilter = searchTerm;
                    string search = searchTerm.ToLower();
                    ordersQuery = ordersQuery.Where(o => o.OrderNumber.Contains(searchTerm) || o.CustomerName.ToLower().Contains(search));
                }
                return View(await ordersQuery.OrderByDescending(o => o.OrderDate).ToListAsync());
            }
        }

        // --- УПРАВЛЕНИЕ ТОВАРАМИ ---
        public async Task<ActionResult> Products()
        {
            if (Session["UserRole"]?.ToString() != "Admin") return RedirectToAction("Login");
            using (var db = new TechZoneContext()) return View(await db.Products.ToListAsync());
        }

        public ActionResult CreateProduct() => Session["UserRole"]?.ToString() == "Admin" ? (ActionResult)View() : RedirectToAction("Login");

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                using (var db = new TechZoneContext())
                {
                    db.Products.Add(product);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Products");
                }
            }
            return View(product);
        }

        public async Task<ActionResult> EditProduct(int? id)
        {
            if (Session["UserRole"]?.ToString() != "Admin") return RedirectToAction("Login");
            if (id == null) return RedirectToAction("Products");
            using (var db = new TechZoneContext())
            {
                var product = await db.Products.FindAsync(id);
                if (product == null) return HttpNotFound();
                return View(product);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                using (var db = new TechZoneContext())
                {
                    db.Entry(product).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return RedirectToAction("Products");
                }
            }
            return View(product);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            if (Session["UserRole"]?.ToString() != "Admin") return RedirectToAction("Login");
            using (var db = new TechZoneContext())
            {
                var product = await db.Products.FindAsync(id);
                if (product != null) { db.Products.Remove(product); await db.SaveChangesAsync(); }
                return RedirectToAction("Products");
            }
        }

        // --- УПРАВЛЕНИЕ ПОЛЬЗОВАТЕЛЯМИ ---
        public async Task<ActionResult> Users(string searchTerm, string role)
        {
            if (Session["UserRole"]?.ToString() != "Admin") return RedirectToAction("Login");
            using (var db = new TechZoneContext())
            {
                var usersQuery = db.Users.AsQueryable();
                ViewBag.CurrentSearch = searchTerm;
                ViewBag.CurrentRole = string.IsNullOrEmpty(role) ? "Все" : role;

                if (!string.IsNullOrEmpty(searchTerm))
                    usersQuery = usersQuery.Where(u => u.Login.Contains(searchTerm) || u.Email.Contains(searchTerm));

                if (!string.IsNullOrEmpty(role) && role != "Все")
                    usersQuery = usersQuery.Where(u => u.Role == role);

                return View(await usersQuery.ToListAsync());
            }
        }

        public ActionResult CreateUser() => Session["UserRole"]?.ToString() == "Admin" ? (ActionResult)View() : RedirectToAction("Login");

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateUser(User user)
        {
            if (Session["UserRole"]?.ToString() != "Admin") return RedirectToAction("Login");
            if (ModelState.IsValid)
            {
                using (var db = new TechZoneContext())
                {
                    if (await db.Users.AnyAsync(u => u.Login == user.Login)) { ModelState.AddModelError("", "Логин занят"); return View(user); }
                    db.Users.Add(user);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Users");
                }
            }
            return View(user);
        }

        public async Task<ActionResult> EditUser(int? id)
        {
            if (Session["UserRole"]?.ToString() != "Admin") return RedirectToAction("Login");
            if (id == null) return RedirectToAction("Users");
            using (var db = new TechZoneContext())
            {
                var user = await db.Users.FindAsync(id);
                if (user == null) return HttpNotFound();
                return View(user);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditUser(User user)
        {
            if (Session["UserRole"]?.ToString() != "Admin") return RedirectToAction("Login");
            if (ModelState.IsValid)
            {
                using (var db = new TechZoneContext())
                {
                    db.Entry(user).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return RedirectToAction("Users");
                }
            }
            return View(user);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteUser(int id)
        {
            if (Session["UserRole"]?.ToString() != "Admin") return RedirectToAction("Login");
            using (var db = new TechZoneContext())
            {
                var user = await db.Users.FindAsync(id);
                if (user != null && user.Login != Session["UserName"]?.ToString()) { db.Users.Remove(user); await db.SaveChangesAsync(); }
                return RedirectToAction("Users");
            }
        }

        // --- ИНФОРМАЦИОННЫЕ СТРАНИЦЫ ---
        public ActionResult Settings() => View();
        public ActionResult Contacts() => View();
        public ActionResult Reviews() => View();
        public ActionResult Delivery() => View();
    }
}