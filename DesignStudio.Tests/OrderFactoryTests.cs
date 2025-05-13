using DesignStudio.BLL;
using DesignStudio.DAL.Models;
using AutoFixture.Xunit2;

namespace DesignStudio.Tests
{
    public class OrderFactoryTests
    {
        private readonly OrderFactory _factory = new OrderFactory();

        [Theory, SafeAutoData]
        public void CreateTurnkeyOrder_SetsAllFieldsCorrectly(
            string customer,
            string phone,
            string designRequirement,
            string designDescription)
        {
            // Act
            var order = _factory.CreateTurnkeyOrder(customer, phone, designRequirement, designDescription);

            // Assert
            Assert.Equal(customer, order.CustomerName);
            Assert.Equal(phone, order.Phone);
            Assert.True(order.IsTurnkey);
            Assert.Equal(designRequirement, order.DesignRequirement);
            Assert.Equal(designDescription, order.DesignDescription);
            Assert.NotEqual(default, order.OrderDate);
        }

        [Theory, SafeAutoData]
        public void CreateServiceOrder_AddsServiceAndSetsFields(
            string customer,
            string phone,
            DesignService service)
        {
            // Act
            var order = _factory.CreateServiceOrder(customer, phone, service);

            // Assert
            Assert.Equal(customer, order.CustomerName);
            Assert.Equal(phone, order.Phone);
            Assert.False(order.IsTurnkey);
            Assert.Contains(service, order.DesignServices);
            Assert.NotEqual(default, order.OrderDate);
        }
    }
}
