using System;
using DesignStudio.BLL.Facades;
using DesignStudio.DAL.Models;
using DesignStudio.UI.CommandPattern;

namespace DesignStudio.UI.UIModules
{
    public static class ServiceUI
    {
        public static void AddService(DesignStudioService service)
        {
            UIHelpers.SafeClear();
            Console.WriteLine("=== Додати послугу ===");
            Console.Write("Назва: ");
            var name = Console.ReadLine();
            Console.Write("Опис: ");
            var desc = Console.ReadLine();
            Console.Write("Ціна: ");
            if (decimal.TryParse(Console.ReadLine(), out decimal price))
            {
                // Створення об'єкта послуги
                var designService = new DesignService { Name = name, Description = desc, Price = price };

                // Створення конкретної команди для додавання послуги
                ICommand command = new AddServiceCommand(service, designService);

                // Використання інвокера для виконання команди
                var invoker = new CommandInvoker();
                invoker.Invoke(command);
            }
            else
            {
                Console.WriteLine("Невірна ціна.");
            }
        }

        public static void UpdateService(DesignStudioService service)
        {
            UIHelpers.SafeClear();
            Console.WriteLine("=== Оновити або Видалити послугу ===");

            var services = service.GetAllServices().ToList();
            if (!services.Any())
            {
                Console.WriteLine("Немає доступних послуг.");
                return;
            }

            // Відображаємо за індексом
            for (int i = 0; i < services.Count; i++)
                Console.WriteLine($"{i + 1}) {services[i].Name} - {services[i].Price} грн");

            Console.Write("Виберіть послугу: ");
            if (!int.TryParse(Console.ReadLine(), out int idx) || idx < 1 || idx > services.Count)
            {
                Console.WriteLine("Невірний вибір.");
                return;
            }

            var selected = services[idx - 1];

            Console.WriteLine("1) Змінити назву");
            Console.WriteLine("2) Змінити опис");
            Console.WriteLine("3) Змінити ціну");
            Console.WriteLine("4) Видалити послугу");
            Console.Write("Оберіть дію: ");
            var act = Console.ReadLine();

            switch (act)
            {
                case "1":
                    Console.Write("Нова назва: ");
                    selected.Name = Console.ReadLine() ?? selected.Name;
                    service.UpdateService(selected);
                    Console.WriteLine("Назву оновлено.");
                    break;
                case "2":
                    Console.Write("Новий опис: ");
                    selected.Description = Console.ReadLine() ?? selected.Description;
                    service.UpdateService(selected);
                    Console.WriteLine("Опис оновлено.");
                    break;
                case "3":
                    Console.Write("Нова ціна: ");
                    if (decimal.TryParse(Console.ReadLine(), out decimal newPrice))
                    {
                        selected.Price = newPrice;
                        service.UpdateService(selected);
                        Console.WriteLine("Ціну оновлено.");
                    }
                    else Console.WriteLine("Невірна ціна.");
                    break;
                case "4":
                    service.DeleteService(selected.DesignServiceId);
                    Console.WriteLine("Послугу видалено.");
                    break;
                default:
                    Console.WriteLine("Невірна дія.");
                    break;
            }
        }
    }
}
