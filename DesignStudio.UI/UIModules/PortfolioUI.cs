using System;
using System.Linq;
using DesignStudio.BLL.Facades;

namespace DesignStudio.UI.UIModules
{
    public static class PortfolioUI
    {
        public static void ShowPortfolio(DesignStudioService service)
        {
            UIHelpers.SafeClear();
            Console.WriteLine("=== Портфоліо ===");
            var items = service.GetPortfolio().ToList();
            if (!items.Any())
            {
                Console.WriteLine("Портфоліо порожнє.");
            }
            else
            {
                foreach (var item in items)
                {
                    Console.WriteLine($"Назва: {item.Title}");
                    Console.WriteLine($"Опис: {item.Description}");
                    if (item.DesignService != null)
                        Console.WriteLine($"Послуга: {item.DesignService.Name} — {item.DesignService.Price} грн");
                    Console.WriteLine(new string('-', 40));
                }
            }
        }
    }
}
