using System.Collections.Generic;
using DesignStudio.DAL.Data;
using DesignStudio.DAL.Models;
using DesignStudio.BLL.Services;

namespace DesignStudio.BLL.Facades
{
    public class DesignStudioService
    {
        private readonly ServiceManager _serviceManager;
        private readonly OrderManager _orderManager;
        private readonly PortfolioManager _portfolioManager;

        public DesignStudioService(DesignStudioContext context, IOrderFactory orderFactory)
        {
            _serviceManager   = new ServiceManager(context);
            _orderManager     = new OrderManager(context, orderFactory);
            _portfolioManager = new PortfolioManager(context);
        }

        // --- Services ---
        public void AddService(DesignService service)
            => _serviceManager.AddDesignService(service);

        public IEnumerable<DesignService> GetAllServices()
            => _serviceManager.GetAllServices();

        public void UpdateService(DesignService service)
            => _serviceManager.UpdateDesignService(service);

        public void DeleteService(int id)
            => _serviceManager.DeleteDesignService(id);
        
        public DesignService? GetServiceById(int id)
        => _serviceManager.GetServiceById(id);

        // --- Orders ---
        public void CreateTurnkeyOrder(string customer, string phone, string requirement, string description)
            => _orderManager.CreateTurnkeyOrder(customer, phone, requirement, description);

        public void CreateServiceOrder(string customer, string phone, int serviceId)
            => _orderManager.CreateServiceOrder(customer, phone, serviceId);

        public IEnumerable<Order> GetAllOrders()
            => _orderManager.GetAllOrders();

        public void DeleteOrder(int id)
            => _orderManager.DeleteOrder(id);

        public void MarkOrderAsCompleted(int id)
            => _orderManager.MarkOrderAsCompleted(id);

        // --- Portfolio ---
        public IEnumerable<PortfolioItem> GetPortfolio()
            => _portfolioManager.GetPortfolio();
    }
}