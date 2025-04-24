using System;
using System.Linq;
using DesignStudio.BLL.DTOs;
using DesignStudio.BLL.Interfaces;
using DesignStudio.UI.CommandPattern;

namespace DesignStudio.UI.UIModules
{
    public static class ServiceUI
    {
        public static void AddService(IDesignStudioService service)
        {
            UIHelpers.SafeClear();
            Console.WriteLine("=== Додати послугу ===");
            Console.Write("Назва: ");
            var name = Console.ReadLine() ?? "";
            Console.Write("Опис: ");
            var desc = Console.ReadLine() ?? "";
            Console.Write("Ціна: ");
            if (decimal.TryParse(Console.ReadLine(), out decimal price))
            {
                var dto = new DesignServiceDto
                {
                    Name = name,
                    Description = desc,
                    Price = price
                };
                var cmd = new AddServiceCommand(service, dto);
                var invoker = new CommandInvoker();
                invoker.Invoke(cmd);
                Console.WriteLine(" Послугу додано.");
            }
            else
            {
                Console.WriteLine(" Невірна ціна.");
            }
        }

        public static void UpdateService(IDesignStudioService service)
        {
            UIHelpers.SafeClear();
            Console.WriteLine("=== Оновити або Видалити послугу ===");
            var list = service.GetServicesAsync().Result.ToList();
            if (!list.Any())
            {
                Console.WriteLine("Немає доступних послуг.");
                return;
            }

            for (int i = 0; i < list.Count; i++)
                Console.WriteLine($"{i + 1}) {list[i].Name} - {list[i].Price} грн");

            Console.Write("Виберіть послугу: ");
            if (!int.TryParse(Console.ReadLine(), out int idx) || idx < 1 || idx > list.Count)
            {
                Console.WriteLine("Невірний вибір.");
                return;
            }

            var dto = list[idx - 1];
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
                    dto.Name = Console.ReadLine() ?? dto.Name;
                    service.UpdateServiceAsync(dto).Wait();
                    Console.WriteLine("✔ Назву оновлено.");
                    break;
                case "2":
                    Console.Write("Новий опис: ");
                    dto.Description = Console.ReadLine() ?? dto.Description;
                    service.UpdateServiceAsync(dto).Wait();
                    Console.WriteLine(" Опис оновлено.");
                    break;
                case "3":
                    Console.Write("Нова ціна: ");
                    if (decimal.TryParse(Console.ReadLine(), out decimal np))
                    {
                        dto.Price = np;
                        service.UpdateServiceAsync(dto).Wait();
                        Console.WriteLine(" Ціну оновлено.");
                    }
                    else Console.WriteLine(" Невірна ціна.");
                    break;
                case "4":
                    service.DeleteServiceAsync(dto.Id).Wait();
                    Console.WriteLine("✔ Послугу видалено.");
                    break;
                default:
                    Console.WriteLine("Невірна дія.");
                    break;
            }
        }
    }
}
