// File: OrderManagerTests.cs
using System.Linq.Expressions;
using AutoFixture;
using AutoFixture.AutoNSubstitute;
using DesignStudio.BLL.DTOs;
using DesignStudio.BLL.Services;
using DesignStudio.DAL.Models;
using NSubstitute;

namespace DesignStudio.Tests
{
    public class OrderManagerTests : TestBase
    {
        private readonly OrderManager _manager;
        private readonly IFixture _fixture;

        public OrderManagerTests()
        {
            _manager = new OrderManager(Uow, Mapper);

            // Налаштовуємо AutoFixture з NSubstitute і прибираємо рекурсії
            _fixture = new Fixture()
                .Customize(new AutoNSubstituteCustomization { ConfigureMembers = true });
            var toRemove = _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList();
            foreach (var b in toRemove) _fixture.Behaviors.Remove(b);
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public async Task GetOrdersAsync_ReturnsMappedDtos()
        {
            // Arrange
            var orders = _fixture.Create<List<Order>>();
            Uow.Orders.GetAllAsync()
               .Returns(Task.FromResult((IEnumerable<Order>)orders));

            // Act
            var dtos = await _manager.GetOrdersAsync();

            // Assert
            Assert.Equal(orders.Count, dtos.Count());
            for (int i = 0; i < orders.Count; i++)
            {
                Assert.Equal(orders[i].OrderId,    dtos.ElementAt(i).Id);
                Assert.Equal(orders[i].CustomerName, dtos.ElementAt(i).CustomerName);
            }
        }

        [Fact]
        public async Task CreateTurnkeyOrderAsync_CommitsTransaction()
        {
            // Arrange
            var dto = _fixture.Build<OrderDto>()
                .With(x => x.IsTurnkey, true)
                .Create();

            // Act
            await _manager.CreateTurnkeyOrderAsync(dto);

            // Assert
            await Uow.Received(1).BeginTransactionAsync();
            await Uow.Orders.Received(1).AddAsync(Arg.Any<Order>());
            await Uow.Received(1).CommitTransactionAsync();
        }

        [Fact]
        public async Task CreateServiceOrderAsync_CommitsTransaction()
        {
            // Arrange
            var serviceEntity = _fixture.Create<DesignService>();
            var dto = _fixture.Build<OrderDto>()
                .With(x => x.IsTurnkey, false)
                .With(x => x.Services, new List<DesignServiceDto> { new DesignServiceDto { Id = serviceEntity.DesignServiceId } })
                .Create();
            Uow.Services.GetByIdAsync(serviceEntity.DesignServiceId)
                .Returns(Task.FromResult(serviceEntity));

            // Act
            await _manager.CreateServiceOrderAsync(dto);

            // Assert
            await Uow.Received(1).BeginTransactionAsync();
            await Uow.Orders.Received(1).AddAsync(Arg.Any<Order>());
            await Uow.Received(1).CommitTransactionAsync();
        }

        [Fact]
        public async Task MarkOrderCompletedAsync_Turnkey_AddsPortfolioItem()
        {
            // Arrange
            var order = _fixture.Build<Order>()
                .With(x => x.IsTurnkey, true)
                .Create();
            Uow.Orders.GetWithIncludeAsync(Arg.Any<Expression<System.Func<Order, object>>>())
               .Returns(Task.FromResult((IEnumerable<Order>)new[] { order }));

            // Act
            await _manager.MarkOrderCompletedAsync(order.OrderId);

            // Assert
            await Uow.Portfolio.Received(1)
                .AddAsync(Arg.Is<PortfolioItem>(pi =>
                    pi.Title == order.DesignRequirement &&
                    pi.Description == order.DesignDescription));
            await Uow.Received(1).CommitTransactionAsync();
        }

        [Fact]
        public async Task DeleteOrderAsync_RemovesAndCommits()
        {
            // Arrange
            var order = _fixture.Create<Order>();
            Uow.Orders.GetByIdAsync(order.OrderId)
               .Returns(Task.FromResult(order));

            // Act
            await _manager.DeleteOrderAsync(order.OrderId);

            // Assert
            Uow.Orders.Received(1).Remove(order);
            await Uow.Received(1).CommitAsync();
        }
    }
}
