using System.Collections.Generic;
using System.Threading.Tasks;
using DesignStudio.BLL.DTOs;

namespace DesignStudio.BLL.Interfaces
{
    public interface IOrderManager
    {
        Task<IEnumerable<OrderDto>> GetOrdersAsync();
        Task CreateTurnkeyOrderAsync(OrderDto dto);
        Task CreateServiceOrderAsync(int serviceId, string customer, string phone);
        Task DeleteOrderAsync(int id);
        Task MarkOrderCompletedAsync(int orderId);
    }
}
