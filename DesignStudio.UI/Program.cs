using System;
using Microsoft.EntityFrameworkCore;
using DesignStudio.DAL.Data;
using DesignStudio.BLL;
using DesignStudio.BLL.Facades;

namespace DesignStudio.UI
{
    internal class Program
    {
        static void Main()
        {
            // Ініціалізація контексту та залежностей
            var options = new DbContextOptionsBuilder<DesignStudioContext>()
                .UseLazyLoadingProxies()  
                .UseSqlite("Data Source=designstudio.db")
                .Options;
            using var context = new DesignStudioContext(options);
            context.Database.EnsureCreated();

            var orderFactory = new OrderFactory();
            var designService = new DesignStudioService(context, orderFactory);

            // Передаємо керування до MenuManager
            MenuManager menu = new MenuManager(designService);
            menu.Run();
        }
    }
}
