using System.Collections.Generic;
using System.Threading.Tasks;
using DesignStudio.BLL.DTOs;
using DesignStudio.BLL.Interfaces;

namespace DesignStudio.BLL.Services
{
    public class DesignStudioService : IDesignStudioService
    {
        private readonly IOrderManager _orders;
        private readonly IServiceManager _services;
        private readonly IPortfolioManager _portfolio;

        public DesignStudioService(
            IOrderManager orders,
            IServiceManager services,
            IPortfolioManager portfolio)
        {
            _orders = orders;
            _services = services;
            _portfolio = portfolio;
        }

        // Services
        public Task<IEnumerable<DesignServiceDto>> GetServicesAsync() =>
            _services.GetServicesAsync();

        public Task AddServiceAsync(DesignServiceDto dto) =>
            _services.AddServiceAsync(dto);

        public Task UpdateServiceAsync(DesignServiceDto dto) =>
            _services.UpdateServiceAsync(dto);

        public Task DeleteServiceAsync(int id) =>
            _services.DeleteServiceAsync(id);

        // Orders
        public Task<IEnumerable<OrderDto>> GetOrdersAsync() =>
            _orders.GetOrdersAsync();

        public Task CreateTurnkeyOrderAsync(OrderDto dto) =>
            _orders.CreateTurnkeyOrderAsync(dto);

        public Task CreateServiceOrderAsync(OrderDto dto) =>
            _orders.CreateServiceOrderAsync(dto);

        public Task MarkOrderCompletedAsync(int orderId) =>
            _orders.MarkOrderCompletedAsync(orderId);

        public Task DeleteOrderAsync(int id) =>
            _orders.DeleteOrderAsync(id);

        // Portfolio
        public Task<IEnumerable<PortfolioItemDto>> GetPortfolioAsync() =>
            _portfolio.GetPortfolioAsync();
    }
}
