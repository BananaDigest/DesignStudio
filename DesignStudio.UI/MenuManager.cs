using DesignStudio.BLL.Interfaces;

namespace DesignStudio.UI
{
    public class MenuManager
    {
        private readonly IDesignStudioService _service;

    public MenuManager(IDesignStudioService service)
    {
        _service = service;
    }

        public void Run()
        {
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
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        UIModules.ServiceUI.AddService(_service);
                        break;
                    case "2":
                        UIModules.ServiceUI.UpdateService(_service);
                        break;
                    case "3":
                        UIModules.OrderUI.MakeOrder(_service);
                        break;
                    case "4":
                        UIModules.PortfolioUI.ShowPortfolio(_service);
                        break;
                    case "5":
                        UIModules.OrderUI.ShowOrders(_service);
                        break;
                    case "6":
                        return;
                    default:
                        Console.WriteLine("Невірна опція.");
                        break;
                }
                UIHelpers.SafeReadKey();
            }
        }
    }
}
