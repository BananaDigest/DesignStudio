using System;
using Microsoft.EntityFrameworkCore;
using DesignStudio.DAL.Data;
using DesignStudio.BLL.Services;
using DesignStudio.BLL.Factories;
using DesignStudio.DAL.Models;

namespace DesignStudio.UI
{
    internal class Program
    {
        static void Main()
        {
            var options = new DbContextOptionsBuilder<DesignStudioContext>()
                .UseSqlite("Data Source=designstudio.db")
                .Options;

            using var context = new DesignStudioContext(options);
            context.Database.EnsureCreated();

            var factory = new OrderFactory();
            var service = new DesignStudioService(context, factory);

            while (true)
            {
                UIHelpers.SafeClear();
                Console.WriteLine("=== Головне Меню ===");
                Console.WriteLine("1) Додати послугу");
                Console.WriteLine("2) Оновити послугу");
                Console.WriteLine("3) Зробити замовлення на дизайн");
                Console.WriteLine("4) Переглянути портфоліо");
                Console.WriteLine("5) Переглянути замовлення");
                Console.WriteLine("6) Вийти");
                Console.Write("Оберіть опцію: ");
                var input = Console.ReadLine();

                switch (input)
                {
                    case "1": AddService(service); break;
                    case "2": UpdateService(service); break;
                    case "3": MakeOrder(service); break;
                    case "4": ShowPortfolio(service); break;
                    case "5": ShowOrders(service); break;
                    case "6": return;
                    default: Console.WriteLine("Невірна опція."); break;
                }

                UIHelpers.SafeReadKey();
            }
        }

        static void AddService(DesignStudioService service)
        {
            UIHelpers.SafeClear();
            Console.WriteLine("=== Додати послугу ===");
            Console.Write("Назва: ");
            var name = Console.ReadLine();
            Console.Write("Опис: ");
            var desc = Console.ReadLine();
            Console.Write("Ціна: ");
            if (decimal.TryParse(Console.ReadLine(), out var price))
            {
                service.AddDesignService(new DesignService { Name = name, Description = desc, Price = price });
                Console.WriteLine(" Послугу додано.");
            }
            else Console.WriteLine(" Невірна ціна.");
        }

        static void UpdateService(DesignStudioService service)
        {
            UIHelpers.SafeClear();
            Console.WriteLine("=== Оновити послугу ===");
            foreach (var s in service.GetAllServices())
                Console.WriteLine($"{s.DesignServiceId}) {s.Name} - {s.Price} грн");

            Console.Write("ID для оновлення: ");
            if (int.TryParse(Console.ReadLine(), out var id))
            {
                var s = service.GetServiceById(id);
                if (s != null)
                {
                    Console.Write("Нова ціна: ");
                    if (decimal.TryParse(Console.ReadLine(), out var price))
                    {
                        s.Price = price;
                        service.UpdateDesignService(s);
                        Console.WriteLine("✔ Оновлено.");
                    }
                    else Console.WriteLine("❌ Невірна ціна.");
                }
                else Console.WriteLine("❌ Послугу не знайдено.");
            }
        }

        static void MakeOrder(DesignStudioService service)
        {
            UIHelpers.SafeClear();
            Console.WriteLine("=== Зробити замовлення ===");
            Console.WriteLine("1) Під ключ\n2) З переліку");
            var type = Console.ReadLine();

            Console.Write("Ім’я/компанія: ");
            var name = Console.ReadLine();
            Console.Write("Телефон: ");
            var phone = Console.ReadLine();

            if (type == "1")
            {
                Console.Write("Що потрібно спроєктувати: ");
                var what = Console.ReadLine();
                Console.Write("Опис: ");
                var desc = Console.ReadLine();
                service.CreateTurnkeyOrder(name, phone, what, desc);
                Console.WriteLine("✔ Замовлення створено.");
            }
            else if (type == "2")
            {
                Console.WriteLine("=== Доступні послуги ===");
                foreach (var s in service.GetAllServices())
                    Console.WriteLine($"{s.DesignServiceId}) {s.Name} — {s.Price} грн");

                Console.Write("ID послуги: ");
                if (int.TryParse(Console.ReadLine(), out var sid))
                {
                    service.CreateServiceOrder(name, phone, sid);
                    Console.WriteLine("✔ Замовлення створено.");
                }
                else Console.WriteLine("❌ Невірний ID.");
            }
            else Console.WriteLine("❌ Невірний вибір.");
        }

        static void ShowOrders(DesignStudioService service)
        {
            UIHelpers.SafeClear();
            Console.WriteLine("=== Замовлення ===");
            foreach (var o in service.GetAllOrders())
            {
                var type = o.IsTurnkey ? "Під ключ" : "З переліку";
                Console.WriteLine($"ID: {o.OrderId} | {o.CustomerName} | {type} | {o.Status}");
            }

            Console.WriteLine("\n1) Скасувати\n2) Позначити як виконане\n3) Назад");
            var act = Console.ReadLine();

            if (act == "1")
            {
                Console.Write("ID: ");
                if (int.TryParse(Console.ReadLine(), out var id)) service.DeleteOrder(id);
            }
            else if (act == "2")
            {
                Console.Write("ID: ");
                if (int.TryParse(Console.ReadLine(), out var id)) service.MarkOrderAsCompleted(id);
            }
        }
        static void ShowPortfolio(DesignStudioService service)
        {
            UIHelpers.SafeClear();
            Console.WriteLine("=== Портфоліо ===");

            var items = service.GetPortfolio().ToList();
            if (items.Count == 0)
            {
                Console.WriteLine("Портфоліо порожнє.");
            }
            else
            {
                foreach (var item in items)
                {
                    Console.WriteLine($"🖼  Назва: {item.Title}");
                    Console.WriteLine($"📄 Опис: {item.Description}");
                    if (item.DesignService != null)
                    {
                        Console.WriteLine($"🔗 Послуга: {item.DesignService.Name} — {item.DesignService.Price} грн");
                    }
                    Console.WriteLine(new string('-', 40));
                }
            }

            UIHelpers.SafeReadKey();
        }

    }
}
