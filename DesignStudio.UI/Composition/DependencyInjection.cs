using Autofac;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using DesignStudio.DAL.Data;
using DesignStudio.DAL.Repositories;
using DesignStudio.BLL.Mapping;
using DesignStudio.BLL.Interfaces;
using DesignStudio.BLL.Services;
using DesignStudio.UI;

namespace DesignStudio.Composition
{
    public class DependencyInjection : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var dbFilePath = Path.Combine(Environment.CurrentDirectory, "designstudio.db");
            builder
                .Register(c =>
                {
                    var options = new DbContextOptionsBuilder<DesignStudioContext>()
                        .UseSqlite($"Data Source={dbFilePath}")
                        .Options;
                    var ctx = new DesignStudioContext(options);
                    ctx.Database.Migrate();
                    return ctx;
                })
                .As<IDbContext>()
                .InstancePerLifetimeScope();

            builder
                .RegisterType<UnitOfWork>()
                .As<IUnitOfWork>()
                .InstancePerLifetimeScope();

            builder
                .Register(c =>
                {
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.AddProfile<MappingProfile>();
                    });
                    return config.CreateMapper();
                })
                .As<IMapper>()
                .SingleInstance();

            builder.RegisterType<OrderManager>().As<IOrderManager>().InstancePerDependency();
            builder.RegisterType<ServiceManager>().As<IServiceManager>().InstancePerDependency();
            builder.RegisterType<PortfolioManager>().As<IPortfolioManager>().InstancePerDependency();
            builder.RegisterType<DesignStudioService>().As<IDesignStudioService>().InstancePerDependency();

            builder.RegisterType<MenuManager>().InstancePerDependency();
        }
    }
}