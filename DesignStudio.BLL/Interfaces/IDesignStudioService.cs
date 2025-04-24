using System.Collections.Generic;
using System.Threading.Tasks;
using DesignStudio.BLL.DTOs;

namespace DesignStudio.BLL.Interfaces
{
    public interface IDesignStudioService
    {
        Task<IEnumerable<DesignServiceDto>> GetServicesAsync();
        Task AddServiceAsync(DesignServiceDto dto);
        Task UpdateServiceAsync(DesignServiceDto dto);
        Task DeleteServiceAsync(int id);

        Task<IEnumerable<OrderDto>> GetOrdersAsync();
        Task CreateTurnkeyOrderAsync(OrderDto dto);
        Task CreateServiceOrderAsync(int serviceId, string customer, string phone);
        Task MarkOrderCompletedAsync(int orderId);
        Task DeleteOrderAsync(int id);

        Task<IEnumerable<PortfolioItemDto>> GetPortfolioAsync();
    }
}
