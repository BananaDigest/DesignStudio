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
            var uowSub = Substitute.For<IUnitOfWork>();
            var ordersRepo = Substitute.For<IGenericRepository<Order>>();
            var servicesRepo = Substitute.For<IGenericRepository<DesignService>>();
            var portfolioRepo = Substitute.For<IGenericRepository<PortfolioItem>>();
            uowSub.Orders.Returns(ordersRepo);
            uowSub.Services.Returns(servicesRepo);
            uowSub.Portfolio.Returns(portfolioRepo);

            Provider = new ServiceCollection()
                .AddAutoMapper(typeof(MappingProfile).Assembly)
                .AddSingleton<IUnitOfWork>(uowSub)
                .BuildServiceProvider();

            Mapper = Provider.GetRequiredService<IMapper>();
            Uow = uowSub;
        }
    }
}