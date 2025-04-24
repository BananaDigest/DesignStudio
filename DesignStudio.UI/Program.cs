using System;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using DesignStudio.DAL.Data;
using DesignStudio.DAL.Repositories;
using DesignStudio.BLL.Mapping;
using DesignStudio.BLL.Interfaces;
using DesignStudio.BLL.Services;

namespace DesignStudio.UI
{
    internal class Program
    {
        static void Main()
        {
            // --- Налаштування DI-контейнера ---
            var services = new ServiceCollection();

            var dbFilePath = "/Users/macboock/DesignStudio/designstudio.db";
            services.AddDbContext<DesignStudioContext>(options =>
                options.UseSqlite($"Data Source={dbFilePath}"));

            // Щоб UnitOfWork отримав той самий контекст
            services.AddScoped<IDbContext>(provider =>
                provider.GetRequiredService<DesignStudioContext>());
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // BLL layer: AutoMapper та менеджери
            services.AddAutoMapper(typeof(MappingProfile).Assembly);
            services.AddScoped<IOrderManager, OrderManager>();
            services.AddScoped<IServiceManager, ServiceManager>();
            services.AddScoped<IPortfolioManager, PortfolioManager>();

            // Фасад над менеджерами
            services.AddScoped<IDesignStudioService, DesignStudioService>();

            // UI
            services.AddScoped<MenuManager>();

            // --- Збірка контейнера ---
            var provider = services.BuildServiceProvider();

            // --- Ініціалізація бази даних ---
            using (var scope = provider.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<DesignStudioContext>();
                try
                {
                    db.Database.EnsureCreated();
                    Console.WriteLine($"База даних створена/підключена: {dbFilePath}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Помилка при ініціалізації БД: {ex.Message}");
                    return;
                }
            }

            // --- Запускаємо головне меню ---
            var menu = provider.GetRequiredService<MenuManager>();
            menu.Run();
        }
    }
}
