using System;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using DesignStudio.BLL.Mapping;
using DesignStudio.DAL.Models;
using DesignStudio.DAL.Repositories;

namespace DesignStudio.Tests
{
    public abstract class TestBase
    {
        protected readonly IMapper Mapper;
        protected readonly IServiceProvider Provider;
        protected readonly IUnitOfWork Uow;

        protected TestBase()
        {
            // Substitute for UnitOfWork and its internal repositories
            var uowSub = Substitute.For<IUnitOfWork>();
            var ordersRepo = Substitute.For<IGenericRepository<Order>>();
            var servicesRepo = Substitute.For<IGenericRepository<DesignService>>();
            var portfolioRepo = Substitute.For<IGenericRepository<PortfolioItem>>();
            uowSub.Orders.Returns(ordersRepo);
            uowSub.Services.Returns(servicesRepo);
            uowSub.Portfolio.Returns(portfolioRepo);

            // Configure DI container with AutoMapper and the IUnitOfWork substitute
            Provider = new ServiceCollection()
                .AddAutoMapper(typeof(MappingProfile).Assembly)
                .AddSingleton<IUnitOfWork>(uowSub)
                .BuildServiceProvider();

            // Resolve mapper and assign unit of work
            Mapper = Provider.GetRequiredService<IMapper>();
            Uow = uowSub;
        }
    }
}