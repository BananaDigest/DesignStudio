using System;
using Microsoft.EntityFrameworkCore;
using DesignStudio.DAL.Data;
using DesignStudio.DAL.Models;
using DesignStudio.BLL;

namespace DesignStudio.UI
{
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

                var newService = new DesignService
                {
                    Name = "Web Design",
                    Description = "Професійний веб-дизайн",
                    Price = 500
                };
                studioService.AddDesignService(newService);
                Console.WriteLine("Нова послуга дизайну додана.");

                var services = studioService.GetDesignServices();
                Console.WriteLine("Список послуг дизайну:");
                foreach (var ds in services)
                {
                    Console.WriteLine($"{ds.DesignServiceId}: {ds.Name} - {ds.Description} - Ціна: {ds.Price}");
                }

                Console.WriteLine("Введіть ID послуги для оновлення:");
                if (int.TryParse(Console.ReadLine(), out int updateId))
                {
                    var dsToUpdate = context.DesignServices.Find(updateId);
                    if (dsToUpdate != null)
                    {
                        dsToUpdate.Price += 50;
                        studioService.UpdateDesignService(dsToUpdate);
                        Console.WriteLine("Послуга оновлена.");
                    }
                    else
                    {
                        Console.WriteLine("Послугу не знайдено.");
                    }
                }
                
                Console.WriteLine("Введіть ID послуги для видалення:");
                if (int.TryParse(Console.ReadLine(), out int deleteId))
                {
                    studioService.DeleteDesignService(deleteId);
                    Console.WriteLine("Послуга видалена (якщо існувала).");
                }
            }
        }
    }
}
