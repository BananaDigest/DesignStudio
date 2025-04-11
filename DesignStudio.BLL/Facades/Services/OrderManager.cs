using System.Linq;
using Microsoft.EntityFrameworkCore;
using DesignStudio.DAL.Data;
using DesignStudio.DAL.Models;

namespace DesignStudio.BLL.Services
{
    public class OrderManager
    {
        private readonly DesignStudioContext _context;
        private readonly IOrderFactory _orderFactory;

        public OrderManager(DesignStudioContext context, IOrderFactory orderFactory)
        {
            _context = context;
            _orderFactory = orderFactory;
        }

        public void CreateTurnkeyOrder(string customer, string phone, string req, string desc)
        {
            var order = _orderFactory.CreateTurnkeyOrder(customer, phone, req, desc);
            _context.Orders.Add(order);
            _context.SaveChanges();
        }

        public void CreateServiceOrder(string customer, string phone, int serviceId)
        {
            var service = _context.DesignServices.Find(serviceId);
            if (service != null)
            {
                var order = _orderFactory.CreateServiceOrder(customer, phone, service);
                _context.Orders.Add(order);
                _context.SaveChanges();
            }
        }

        public IEnumerable<Order> GetAllOrders()
        {
            return _context.Orders
                .Include(o => o.DesignServices)
                .ToList();
        }

        public void DeleteOrder(int id)
        {
            var order = _context.Orders.Find(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                _context.SaveChanges();
            }
        }

        public void MarkOrderAsCompleted(int orderId)
       {
           var order = _context.Orders
               .Include(o => o.DesignServices)
               .FirstOrDefault(o => o.OrderId == orderId);
  
           _context.Orders.Update(order);
  
           var portfolioItem = order.IsTurnkey
               ? new PortfolioItem
               {
                   Title = order.DesignRequirement ?? "Проєкт",
                   Description = order.DesignDescription ?? "Опис відсутній"
               }
               : new PortfolioItem
               {
                   Title = order.DesignServices.First().Name,
                   Description = order.DesignServices.First().Description,
                   DesignService = order.DesignServices.First()
               };
  
           _context.PortfolioItems.Add(portfolioItem);
           _context.SaveChanges();
       }
    }
}
