using System;
using DesignStudio.BLL.Facades;

namespace DesignStudio.UI.UIModules
{
    public static class OrderUI
    {
        public static void MakeOrder(DesignStudioService service)
        {
            UIHelpers.SafeClear();
            Console.WriteLine("=== Зробити замовлення на дизайн ===");
            Console.WriteLine("1) Під ключ");
            Console.WriteLine("2) З переліку");
            var type = Console.ReadLine();

            Console.Write("Ім’я/компанія: ");
            var customer = Console.ReadLine();
            Console.Write("Телефон: ");
            var phone = Console.ReadLine();

            if (type == "1")
            {
                Console.Write("Що потрібно спроєктувати: ");
                var design = Console.ReadLine();
                Console.Write("Опис: ");
                var desc = Console.ReadLine();
                service.CreateTurnkeyOrder(customer, phone, design, desc);
                Console.WriteLine("Замовлення під ключ створено.");
            }
            else if (type == "2")
            {
                Console.WriteLine("=== Доступні послуги ===");
                foreach (var s in service.GetAllServices())
                {
                    Console.WriteLine($"{s.DesignServiceId}) {s.Name} — {s.Price} грн");
                    Console.WriteLine($"    Опис: {s.Description}");
                }

                Console.Write("ID послуги: ");
                if (int.TryParse(Console.ReadLine(), out int sid))
                {
                    service.CreateServiceOrder(customer, phone, sid);
                    Console.WriteLine("Замовлення з переліку створено.");
                }
                else Console.WriteLine("Невірний ID.");
            }
            else
            {
                Console.WriteLine("Невірний вибір типу замовлення.");
            }
        }

        public static void ShowOrders(DesignStudioService service)
        {
            UIHelpers.SafeClear();
            Console.WriteLine("=== Замовлення ===");

            var orders = service.GetAllOrders().ToList();
            if (!orders.Any())
            {
                Console.WriteLine("Замовлень немає.");
                return;
            }

            foreach (var o in orders)
            {
                Console.WriteLine($"ID замовлення: {o.OrderId}");
                Console.WriteLine($"Замовник: {o.CustomerName}");
                Console.WriteLine($"Телефон: {o.Phone}");
                Console.WriteLine($"Дата: {o.OrderDate}");
                var type = o.IsTurnkey ? "Під ключ" : "З переліку";
                Console.WriteLine($"Тип: {type}");
                if (o.IsTurnkey)
                {
                    Console.WriteLine($"Проєкт: {o.DesignRequirement}");
                    Console.WriteLine($"Опис: {o.DesignDescription}");
                }
                else
                {
                    Console.WriteLine("Послуги:");
                    foreach (var s in o.DesignServices)
                    {
                        Console.WriteLine($"  - {s.Name} ({s.Price} грн)");
                        Console.WriteLine($"      Опис: {s.Description}");
                    }
                }
                Console.WriteLine(new string('-', 50));
            }

            Console.WriteLine("\n1) Скасувати замовлення");
            Console.WriteLine("2) Позначити як виконане");
            Console.Write("Оберіть дію: ");
            var act = Console.ReadLine();

            if (act == "1")
            {
                Console.Write("Номер замовлення: ");
                if (int.TryParse(Console.ReadLine(), out int idx) && idx >= 1 && idx <= orders.Count)
                {
                    service.DeleteOrder(orders[idx - 1].OrderId);
                    Console.WriteLine("Замовлення скасовано.");
                }
                else Console.WriteLine("Невірний номер.");
            }
            else if (act == "2")
            {
                Console.Write("Номер замовлення: ");
                if (int.TryParse(Console.ReadLine(), out int idx) && idx >= 1 && idx <= orders.Count)
                {
                    service.MarkOrderAsCompleted(orders[idx - 1].OrderId);
                    Console.WriteLine("Замовлення виконано і додано до портфоліо.");
                }
                else Console.WriteLine("Невірний номер.");
            }
        }
    }
}
