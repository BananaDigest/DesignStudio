using Autofac;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using DesignStudio.DAL.Data;
using DesignStudio.DAL.Repositories;
using DesignStudio.BLL.Mapping;
using DesignStudio.BLL.Interfaces;
using DesignStudio.BLL.Services;
using DesignStudio.UI;
using DesignStudio.UI.CommandPattern;
using DesignStudio.BLL.DTOs;
using DesignStudio.DAL.Models;

namespace DesignStudio.Composition
{
    public class DependencyInjection : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var dbFile = Path.Combine(AppContext.BaseDirectory, "designstudio.db");
            builder.Register(c =>
            {
                var opts = new DbContextOptionsBuilder<DesignStudioContext>()
                    .UseSqlite($"Data Source={dbFile}")
                    .Options;
                var ctx = new DesignStudioContext(opts);
                ctx.Database.Migrate();
                return ctx;
            })
            .As<DesignStudioContext>()
            .As<IDbContext>()
            .InstancePerLifetimeScope();

            builder.RegisterType<UnitOfWork>()
                   .As<IUnitOfWork>()
                   .InstancePerLifetimeScope();

            // Конфігурація AutoMapper
            builder.Register(c =>
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<MappingProfile>(); // Використовуйте ваш профіль мапінгу
                });
                return config.CreateMapper();
            })
            .As<IMapper>()
            .SingleInstance();

            // Реєстрація менеджерів
            builder.RegisterType<ServiceManager>()
                   .As<IServiceManager>()
                   .InstancePerLifetimeScope();
            builder.RegisterType<OrderManager>()
                   .As<IOrderManager>()
                   .InstancePerLifetimeScope();
            builder.RegisterType<PortfolioManager>()
                   .As<IPortfolioManager>()
                   .InstancePerLifetimeScope();

            // Основний сервіс
            builder.RegisterType<DesignStudioService>()
                   .As<IDesignStudioService>()
                   .InstancePerLifetimeScope();

            // Команди та інтерфейси
            builder.RegisterType<CommandInvoker>()
                   .AsSelf() // Реєстрація без інтерфейсу
                   .InstancePerLifetimeScope();
            builder.RegisterType<AddServiceCommand>()
                   .Keyed<ICommand>("AddService")
                   .InstancePerLifetimeScope();

            // Меню
            builder.RegisterType<MenuManager>()
                   .AsSelf()
                   .InstancePerLifetimeScope();
        }
    }
}