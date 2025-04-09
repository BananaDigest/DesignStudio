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
                    Console.WriteLine($"{s.DesignServiceId}) {s.Name} — {s.Price} грн");

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

            // Відображаємо за індексом
            for (int i = 0; i < orders.Count; i++)
            {
                var o = orders[i];
                var type = o.IsTurnkey ? "Під ключ" : "З переліку";
                Console.WriteLine($"{i+1}) {o.CustomerName} | {type}");
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
