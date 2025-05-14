using AutoFixture;
using DesignStudio.BLL;
using DesignStudio.DAL.Models;
using AutoFixture.Kernel;

namespace DesignStudio.Tests
{
    public class OrderFactoryTests
    {
        private readonly OrderFactory _factory;
        private readonly Fixture _fixture;

        public OrderFactoryTests()
        {
            _factory = new OrderFactory();
            _fixture = new Fixture();
            // Remove recursion exception behavior
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>()
                .ToList().ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public void CreateTurnkeyOrder_SetsAllFieldsCorrectly()
        {
            var customer = _fixture.Create<string>();
            var phone = _fixture.Create<string>();
            var req = _fixture.Create<string>();
            var desc = _fixture.Create<string>();

            var order = _factory.CreateTurnkeyOrder(customer, phone, req, desc);

            Assert.Equal(customer, order.CustomerName);
            Assert.Equal(phone, order.Phone);
            Assert.True(order.IsTurnkey);
            Assert.Equal(req, order.DesignRequirement);
            Assert.Equal(desc, order.DesignDescription);
            Assert.NotEqual(default, order.OrderDate);
        }

        [Fact]
        public void CreateServiceOrder_AddsServiceAndSetsFields()
        {
            var customer = _fixture.Create<string>();
            var phone = _fixture.Create<string>();
            var service = _fixture.Create<DesignService>();

            var order = _factory.CreateServiceOrder(customer, phone, service);

            Assert.Equal(customer, order.CustomerName);
            Assert.Equal(phone, order.Phone);
            Assert.False(order.IsTurnkey);
            Assert.Contains(service, order.DesignServices);
            Assert.NotEqual(default, order.OrderDate);
        }
    }
}
