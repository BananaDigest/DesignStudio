using Autofac;
using DesignStudio.Composition;
using DesignStudio.DAL.Data;
using Microsoft.EntityFrameworkCore;


namespace DesignStudio.UI
{
    internal class Program
    {
        static void Main()
        {
            // Налаштування Autofac Container
            var builder = new ContainerBuilder();

            // Реєстрація DesignStudioContext з EnsureCreated
            builder.Register(c =>
            {
                var options = new DbContextOptionsBuilder<DesignStudioContext>()
                    .UseSqlite("Data Source= /Users/macboock/DesignStudio/DesignStudio.API/designstudio.db")
                    .Options;
                var context = new DesignStudioContext(options);
                // Створюємо або оновлюємо базу даних
                context.Database.EnsureCreated();
                return context;
            })
            .As<DesignStudioContext>()
            .As<DesignStudio.DAL.Data.IDbContext>()
            .InstancePerLifetimeScope();

            // Реєстрація інших залежностей через модуль
            builder.RegisterModule<DependencyInjection>();

            var container = builder.Build();

            using (var scope = container.BeginLifetimeScope())
            {
                var menuManager = scope.Resolve<MenuManager>();
                menuManager.Run();
            }
        }
    }
}
