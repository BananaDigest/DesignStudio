using System.Linq.Expressions;
using AutoFixture;
using AutoMapper;
using DesignStudio.BLL.DTOs;
using DesignStudio.BLL.Mapping;
using DesignStudio.BLL.Services;
using DesignStudio.DAL.Models;
using DesignStudio.DAL.Repositories;
using Moq;
using AutoFixture.Kernel;

namespace DesignStudio.Tests
{
    public class OrderManagerTests
    {
        private readonly Mock<IUnitOfWork> _uowMock;
        private readonly IMapper _mapper;
        private readonly OrderManager _manager;
        private readonly Fixture _fixture;

        public OrderManagerTests()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = config.CreateMapper();

            _fixture = new Fixture();
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>()
                .ToList().ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _uowMock = new Mock<IUnitOfWork>(MockBehavior.Strict);
            // Common setups for transactions
            _uowMock.Setup(u => u.BeginTransactionAsync()).Returns(Task.CompletedTask);
            _uowMock.Setup(u => u.CommitTransactionAsync()).Returns(Task.CompletedTask);
            _uowMock.Setup(u => u.CommitAsync()).ReturnsAsync(1);

            var ordersRepo = new Mock<IGenericRepository<Order>>();
            var servicesRepo = new Mock<IGenericRepository<DesignService>>();
            var portfolioRepo = new Mock<IGenericRepository<PortfolioItem>>();

            _uowMock.Setup(u => u.Orders).Returns(ordersRepo.Object);
            _uowMock.Setup(u => u.Services).Returns(servicesRepo.Object);
            _uowMock.Setup(u => u.Portfolio).Returns(portfolioRepo.Object);

            _manager = new OrderManager(_uowMock.Object, _mapper);
        }

        [Fact]
        public async Task GetOrdersAsync_ReturnsMappedDtos()
        {
            var orders = _fixture.CreateMany<Order>(3).ToList();
            _uowMock.Setup(u => u.Orders.GetAllAsync()).ReturnsAsync(orders);

            var dtos = await _manager.GetOrdersAsync();

            Assert.Equal(3, dtos.Count());
            Assert.Equal(orders.First().CustomerName, dtos.First().CustomerName);
        }

        [Fact]
        public async Task CreateTurnkeyOrderAsync_CommitsTransaction()
        {
            var dto = _fixture.Build<OrderDto>()
                .With(x => x.IsTurnkey, true)
                .Create();

            await _manager.CreateTurnkeyOrderAsync(dto);

            _uowMock.Verify(u => u.BeginTransactionAsync(), Times.Once);
            _uowMock.Verify(u => u.Orders.AddAsync(It.IsAny<Order>()), Times.Once);
            _uowMock.Verify(u => u.CommitTransactionAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateServiceOrderAsync_CommitsTransaction()
        {
            var serviceEntity = _fixture.Create<DesignService>();
            var dto = _fixture.Build<OrderDto>()
                .With(x => x.IsTurnkey, false)
                .With(x => x.Services, new List<DesignServiceDto> { new DesignServiceDto { Id = serviceEntity.DesignServiceId } })
                .Create();
            _uowMock.Setup(u => u.Services.GetByIdAsync(serviceEntity.DesignServiceId))
                .ReturnsAsync(serviceEntity);

            await _manager.CreateServiceOrderAsync(dto);

            _uowMock.Verify(u => u.BeginTransactionAsync(), Times.Once);
            _uowMock.Verify(u => u.Orders.AddAsync(It.Is<Order>(o => o.DesignServices.Contains(serviceEntity))), Times.Once);
            _uowMock.Verify(u => u.CommitTransactionAsync(), Times.Once);
        }

        [Fact]
        public async Task MarkOrderCompletedAsync_Turnkey_AddsPortfolioItem()
        {
            var order = _fixture.Build<Order>()
                .With(o => o.IsTurnkey, true)
                .With(o => o.DesignRequirement, "Req")
                .With(o => o.DesignDescription, "Desc")
                .Create();
            _uowMock.Setup(u => u.Orders.GetWithIncludeAsync(It.IsAny<Expression<Func<Order, object>>>()))
                .ReturnsAsync(new[] { order });

            await _manager.MarkOrderCompletedAsync(order.OrderId);

            _uowMock.Verify(u => u.Portfolio.AddAsync(It.Is<PortfolioItem>(pi => pi.Title == "Req" && pi.Description == "Desc")), Times.Once);
            _uowMock.Verify(u => u.CommitTransactionAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteOrderAsync_RemovesAndCommits()
        {
            var order = _fixture.Create<Order>();
            _uowMock.Setup(u => u.Orders.GetByIdAsync(order.OrderId)).ReturnsAsync(order);

            await _manager.DeleteOrderAsync(order.OrderId);

            _uowMock.Verify(u => u.Orders.Remove(order), Times.Once);
            _uowMock.Verify(u => u.CommitAsync(), Times.Once);
        }
    }
}
