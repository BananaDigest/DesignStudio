using System;
using System.Linq;
using DesignStudio.BLL.DTOs;
using DesignStudio.BLL.Interfaces;
using DesignStudio.BLL.Mapping;
using AutoMapper;

namespace DesignStudio.UI.UIModules
{
    public static class OrderUI
    {
        public static void MakeOrder(IDesignStudioService service, IMapper mapper)
        {
            UIHelpers.SafeClear();
            Console.WriteLine("=== Зробити замовлення на дизайн ===");
            Console.WriteLine("1) Під ключ");
            Console.WriteLine("2) З переліку");
            var type = Console.ReadLine();

            Console.Write("Ім’я/компанія: ");
            var customer = Console.ReadLine() ?? "";
            Console.Write("Телефон: ");
            var phone = Console.ReadLine() ?? "";

            if (type == "1")
            {
                Console.Write("Що потрібно спроєктувати: ");
                var req = Console.ReadLine() ?? "";
                Console.Write("Опис: ");
                var desc = Console.ReadLine() ?? "";
                var dto = new OrderDto
                {
                    CustomerName = customer,
                    Phone = phone,
                    OrderDate = DateTime.Now,
                    IsTurnkey = true,
                    DesignRequirement = req,
                    DesignDescription = desc
                };
                service.CreateTurnkeyOrderAsync(dto).Wait();
                Console.WriteLine(" Замовлення під ключ створено.");
            }
            else if (type == "2")
            {
                Console.WriteLine("=== Доступні послуги ===");
                var services = service.GetServicesAsync().Result;
                foreach (var s in services)
                {
                    Console.WriteLine($"{s.Id}) {s.Name} — {s.Price} грн");
                    Console.WriteLine($"    Опис: {s.Description}");
                }

                Console.Write("ID послуги: ");
                if (int.TryParse(Console.ReadLine(), out int sid))
                {
                    var dto = new OrderDto
                    {
                        CustomerName = customer,
                        Phone = phone,
                        OrderDate = DateTime.Now,
                        IsTurnkey = false,
                        Services = new List<DesignServiceDto>
                        {
                            new DesignServiceDto { Id = sid }
                        }
                    };

                    service.CreateServiceOrderAsync(dto).Wait();
                    Console.WriteLine(" Замовлення з переліку створено.");
                }

                else
                {
                    Console.WriteLine("Невірний ID.");
                }
            }
            else
            {
                Console.WriteLine("Невірний вибір типу замовлення.");
            }
        }

        public static void ShowOrders(IDesignStudioService service)
        {
            UIHelpers.SafeClear();
            Console.WriteLine("=== Замовлення ===");
            var orders = service.GetOrdersAsync().Result.ToList();
            if (!orders.Any())
            {
                Console.WriteLine("Замовлень немає.");
                return;
            }

            foreach (var o in orders)
            {
                Console.WriteLine($"ID: {o.Id} | {o.CustomerName} | {(o.IsTurnkey ? "Під ключ" : "З переліку")} | {o.OrderDate}");
            }

            Console.WriteLine("\n1) Скасувати");
            Console.WriteLine("2) Позначити як виконане");
            Console.Write("Ваша дія: ");
            var act = Console.ReadLine();

            if (act == "1" && int.TryParse(Console.ReadLine(), out int delId))
            {
                service.DeleteOrderAsync(delId).Wait();
                Console.WriteLine("✔ Замовлення скасовано.");
            }
            else if (act == "2" && int.TryParse(Console.ReadLine(), out int compId))
            {
                service.MarkOrderCompletedAsync(compId).Wait();
                Console.WriteLine("✔ Замовлення позначено як виконане.");
            }
        }
    }
}
