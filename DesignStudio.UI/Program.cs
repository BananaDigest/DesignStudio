using System;
using Microsoft.EntityFrameworkCore;
using DesignStudio.DAL.Data;
using DesignStudio.DAL.Models;
using DesignStudio.BLL;

namespace DesignStudio.UI
{
        // Консольний UI, який звертається до бізнес-логіки
    class Program
    {
        static void Main(string[] args)
        {
            var options = new DbContextOptionsBuilder<DesignStudioContext>()
                .UseSqlite("Data Source=designstudio.db")
                .Options;

            using (var context = new DesignStudioContext(options))
            {
                context.Database.EnsureCreated();
                var studioService = new DesignStudioService(context);

                while (true)
                {
                    Console.WriteLine("=== Головне Меню ===");
                    Console.WriteLine("1) Додати послугу");
                    Console.WriteLine("2) Оновити послугу");
                    Console.WriteLine("3) Зробити замовлення на дизайн");
                    Console.WriteLine("4) Переглянути замовлення");
                    Console.WriteLine("5) Завершити роботу");
                    Console.Write("Оберіть опцію: ");
                    var choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            AddService(studioService);
                            break;
                        case "2":
                            UpdateService(studioService);
                            break;
                        case "3":
                            MakeOrder(studioService);
                            break;
                        case "4":
                            ViewOrders(studioService);
                            break;
                        case "5":
                            // Завершення роботи – дані збережено в SQLite
                            return;
                        default:
                            Console.WriteLine("Невірний вибір. Спробуйте ще раз.");
                            Console.ReadKey();
                            break;
                    }
                }
            }
        }

        // Функція додавання нової послуги
        static void AddService(DesignStudioService service)
        {
            Console.WriteLine("=== Додавання послуги ===");
            Console.Write("Введіть назву послуги: ");
            string name = Console.ReadLine();
            Console.Write("Введіть опис послуги: ");
            string description = Console.ReadLine();
            Console.Write("Введіть ціну: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal price))
            {
                Console.WriteLine("Некоректне значення ціни.");
                Console.ReadLine();
                return;
            }

            var designService = new DesignService
            {
                Name = name,
                Description = description,
                Price = price
            };

            service.AddDesignService(designService);
            Console.WriteLine("Послуга додана.");
        }

        // Функція оновлення послуги
        static void UpdateService(DesignStudioService service)
        {
            Console.WriteLine("=== Оновлення послуги ===");
            var services = service.GetDesignServices();
            foreach (var s in services)
            {
                Console.WriteLine($"{s.DesignServiceId}: {s.Name} - {s.Price} грн");
            }

            Console.Write("Введіть ID послуги для оновлення: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var selectedService = services.FirstOrDefault(s => s.DesignServiceId == id);
                if (selectedService != null)
                {
                    Console.Write("Введіть нову ціну: ");
                    if (decimal.TryParse(Console.ReadLine(), out decimal newPrice))
                    {
                        selectedService.Price = newPrice;
                        service.UpdateDesignService(selectedService);
                        Console.WriteLine("Послуга оновлена.");
                    }
                    else
                    {
                        Console.WriteLine("Некоректне значення ціни.");
                    }
                }
                else
                {
                    Console.WriteLine("Послугу не знайдено.");
                }
            }
            else
            {
                Console.WriteLine("Некоректне значення ID.");
            }
            Console.WriteLine("Натисніть будь-яку клавішу для повернення до меню...");
            Console.ReadKey();
        }

        // Функція створення замовлення на дизайн
        static void MakeOrder(DesignStudioService service)
        {
            Console.WriteLine("=== Замовлення на дизайн ===");

            // Вивід робіт з портфоліо
            var portfolioItems = service.GetDesignServices()
                                        .SelectMany(ds => ds.PortfolioItems)
                                        .ToList();
            Console.WriteLine("--- Роботи з портфоліо ---");
            if (portfolioItems.Any())
            {
                foreach (var item in portfolioItems)
                {
                    Console.WriteLine($"ID: {item.PortfolioItemId}, Назва: {item.Title}, Опис: {item.Description}");
                }
            }
            else
            {
                Console.WriteLine("Портфоліо поки що порожнє.");
            }
            Console.WriteLine();

            Console.WriteLine("Оберіть тип замовлення:");
            Console.WriteLine("1) Замовлення під ключ");
            Console.WriteLine("2) Замовлення з переліку");
            Console.Write("Ваш вибір: ");
            var orderType = Console.ReadLine();

            Console.Write("Введіть Ваше ім'я або назву компанії: ");
            string customer = Console.ReadLine();
            Console.Write("Введіть контактний номер телефону: ");
            string phone = Console.ReadLine();

            if (orderType == "1")
            {
                // Замовлення під ключ
                Console.Write("Введіть, що потрібно спроєктувати: ");
                string designRequirement = Console.ReadLine();
                Console.Write("Введіть опис дизайну: ");
                string designDescription = Console.ReadLine();

                var order = new Order
                {
                    CustomerName = customer,
                    Phone = phone,
                    OrderDate = DateTime.Now,
                    IsTurnkey = true,
                    DesignRequirement = designRequirement,
                    DesignDescription = designDescription
                };
                service.AddOrder(order);
                Console.WriteLine("Замовлення під ключ додано.");
            }
            else if (orderType == "2")
            {
                // Замовлення з переліку – вивід доступних послуг
                Console.WriteLine("--- Доступні послуги ---");
                var designServices = service.GetDesignServices();
                foreach (var ds in designServices)
                {
                    Console.WriteLine($"{ds.DesignServiceId}: {ds.Name} - {ds.Description} - Ціна: {ds.Price} грн");
                }
                Console.Write("Введіть ID послуги: ");
                if (int.TryParse(Console.ReadLine(), out int serviceId))
                {
                    var selectedService = designServices.FirstOrDefault(ds => ds.DesignServiceId == serviceId);
                    if (selectedService != null)
                    {
                        var order = new Order
                        {
                            CustomerName = customer,
                            Phone = phone,
                            OrderDate = DateTime.Now,
                            IsTurnkey = false
                        };
                        // Додаємо вибрану послугу до замовлення\n                        order.DesignServices.Add(selectedService);
                        service.AddOrder(order);
                        Console.WriteLine("Замовлення з переліку додано.");
                    }
                    else
                    {
                        Console.WriteLine("Послугу не знайдено.");
                    }
                }
                else
                {
                    Console.WriteLine("Некоректно введений ID послуги.");
                }
            }
            else
            {
                Console.WriteLine("Невірний вибір типу замовлення.");
            }

            Console.WriteLine("Натисніть будь-яку клавішу для повернення до меню...");
            Console.ReadKey();
        }

        // Функція перегляду замовлень із можливістю скасування або позначення як виконаних
        static void ViewOrders(DesignStudioService service)
        {
            Console.WriteLine("=== Перегляд замовлень ===");
            var orders = service.GetOrders();
            if (!orders.Any())
            {
                Console.WriteLine("Немає жодного замовлення.");
            }
            else
            {
                foreach (var o in orders)
                {
                    string orderType = o.IsTurnkey ? "Під ключ" : "З переліку";
                    Console.WriteLine($"ID: {o.OrderId}, Замовник: {o.CustomerName}, Телефон: {o.Phone}, Дата: {o.OrderDate}, Тип: {orderType}, Статус: {o.Status}");
                }
                Console.WriteLine();
                Console.WriteLine("Оберіть дію:");
                Console.WriteLine("1) Скасувати замовлення");
                Console.WriteLine("2) Позначити замовлення як виконане (додається в портфоліо)");
                Console.WriteLine("3) Назад");
                Console.Write("Ваш вибір: ");
                var action = Console.ReadLine();
                if (action == "1")
                {
                    Console.Write("Введіть ID замовлення для скасування: ");
                    if (int.TryParse(Console.ReadLine(), out int cancelId))
                    {
                        service.DeleteOrder(cancelId);
                        Console.WriteLine("Замовлення скасовано.");
                    }
                    else
                    {
                        Console.WriteLine("Некоректний ID.");
                    }
                }
                else if (action == "2")
                {
                    Console.Write("Введіть ID замовлення для позначення як виконане: ");
                    if (int.TryParse(Console.ReadLine(), out int completeId))
                    {
                        service.MarkOrderAsCompleted(completeId);
                        Console.WriteLine("Замовлення позначено як виконане та додано в портфоліо.");
                    }
                    else
                    {
                        Console.WriteLine("Некоректний ID.");
                    }
                }
            }
            Console.WriteLine("Натисніть будь-яку клавішу для повернення до меню...");
            Console.ReadKey();
        }
    }
}
