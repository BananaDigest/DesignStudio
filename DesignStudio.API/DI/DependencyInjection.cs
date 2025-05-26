using Autofac;
using AutoMapper;
using DesignStudio.BLL.Interfaces;
using DesignStudio.BLL.Services;
using DesignStudio.DAL.Data;
using DesignStudio.DAL.Repositories;
using DesignStudio.BLL.Mapping;
using Microsoft.EntityFrameworkCore;

namespace DesignStudio.API.DI
{
    public static class DependencyInjection
    {
        public static void AddAutofacServices(this ContainerBuilder builder, IConfiguration configuration)
    {
        // DbContext
        builder.Register(c =>
        {
            var conn = configuration.GetConnectionString("DefaultConnection")
                       ?? throw new InvalidOperationException("Connection string not found");
            var opts = new DbContextOptionsBuilder<DesignStudioContext>()
                .UseSqlite(conn)
                .Options;
            return new DesignStudioContext(opts);
        })
        .As<DesignStudioContext>()
        .As<IDbContext>()
        .InstancePerLifetimeScope();

        // Repository / UnitOfWork
        builder.RegisterType<UnitOfWork>()
               .As<IUnitOfWork>()
               .InstancePerLifetimeScope();

        // AutoMapper
        builder.Register(c =>
        {
            var cfg = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
                cfg.AddProfile<DesignStudio.API.Mapping.ApiMappingProfile>();
            });
            return cfg.CreateMapper();
        })
        .As<IMapper>()
        .SingleInstance();

        // BLL services
        builder.RegisterType<ServiceManager>()
               .As<IServiceManager>()
               .InstancePerLifetimeScope();
        builder.RegisterType<OrderManager>()
               .As<IOrderManager>()
               .InstancePerLifetimeScope();
        builder.RegisterType<PortfolioManager>()
               .As<IPortfolioManager>()
               .InstancePerLifetimeScope();
        builder.RegisterType<DesignStudioService>()
               .As<IDesignStudioService>()
               .InstancePerLifetimeScope();
    }
}
}
