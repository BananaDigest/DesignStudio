using System;
using System.Linq;
using DesignStudio.BLL.Interfaces;

namespace DesignStudio.UI.UIModules
{
    public static class PortfolioUI
    {
        public static void ShowPortfolio(IDesignStudioService service)
        {
            UIHelpers.SafeClear();
            Console.WriteLine("=== Портфоліо ===");
            var items = service.GetPortfolioAsync().Result.ToList();
            if (!items.Any())
            {
                Console.WriteLine("Портфоліо порожнє.");
                return;
            }

            foreach (var i in items)
            {
                Console.WriteLine($"Назва: {i.Title}");
                Console.WriteLine($"Опис: {i.Description}");
                if (i.Service != null)
                    Console.WriteLine($"Послуга: {i.Service.Name} — {i.Service.Price} грн");
                Console.WriteLine(new string('-', 40));
            }
        }
    }
}
