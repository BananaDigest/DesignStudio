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

            // Налаштування шляху до бази даних
            var dbFilePath = Path.Combine(Environment.CurrentDirectory, "designstudio.db");
            services.AddDbContext<DesignStudioContext>(options =>
                options.UseSqlite($"Data Source={dbFilePath}"));

            // Зареєструвати контекст як IDbContext для UnitOfWork
            services.AddScoped<IDbContext>(provider =>
                provider.GetRequiredService<DesignStudioContext>());
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // BLL: AutoMapper та менеджери
            services.AddAutoMapper(typeof(MappingProfile).Assembly);
            services.AddScoped<IOrderManager, OrderManager>();
            services.AddScoped<IServiceManager, ServiceManager>();
            services.AddScoped<IPortfolioManager, PortfolioManager>();

            // Фасад
            services.AddScoped<IDesignStudioService, DesignStudioService>();

            // UI: MenuManager
            services.AddScoped<MenuManager>();

            // --- Будуємо контейнер ---
            var provider = services.BuildServiceProvider();

            // --- Ініціалізація бази даних та перевірка ---
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

            // --- Запуск головного меню ---
            var menuManager = provider.GetRequiredService<MenuManager>();
            menuManager.Run();
        }
    }
}
