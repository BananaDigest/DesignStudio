using System.Collections.Generic;
using System.Linq;
using DesignStudio.DAL.Data;
using DesignStudio.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DesignStudio.BLL
{
     // Клас бізнес-логіки, що інкапсулює операції над даними
    public class DesignStudioService
    {
        private readonly DesignStudioContext _context;

        public DesignStudioService(DesignStudioContext context)
        {
            _context = context;
        }

        // CRUD операції для послуг

        public void AddDesignService(DesignService service)
        {
            _context.DesignServices.Add(service);
            _context.SaveChanges();
        }

        public IEnumerable<DesignService> GetDesignServices()
        {
            return _context.DesignServices
                           .Include(ds => ds.PortfolioItems)
                           .Include(ds => ds.Orders)
                           .ToList();
        }

        public void UpdateDesignService(DesignService service)
        {
            _context.DesignServices.Update(service);
            _context.SaveChanges();
        }

        public void DeleteDesignService(int serviceId)
        {
            var service = _context.DesignServices.Find(serviceId);
            if (service != null)
            {
                _context.DesignServices.Remove(service);
                _context.SaveChanges();
            }
        }

        // Операції для замовлень

        public void AddOrder(Order order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();
        }

        public IEnumerable<Order> GetOrders()
        {
            return _context.Orders
                           .Include(o => o.DesignServices)
                           .ToList();
        }

        public void DeleteOrder(int orderId)
        {
            var order = _context.Orders.Find(orderId);
            if (order != null)
            {
                _context.Orders.Remove(order);
                _context.SaveChanges();
            }
        }

        // Позначення замовлення як виконаного з додаванням роботи в портфоліо
        public void MarkOrderAsCompleted(int orderId)
        {
            var order = _context.Orders.Include(o => o.DesignServices)
                                       .FirstOrDefault(o => o.OrderId == orderId);
            if (order != null && order.Status != OrderStatus.Completed)
            {
                order.Status = OrderStatus.Completed;
                _context.Orders.Update(order);

                PortfolioItem portfolioItem = null;
                if (order.IsTurnkey)
                {
                    // Для замовлення «під ключ» використовуємо введені дані
                    portfolioItem = new PortfolioItem
                    {
                        Title = order.DesignRequirement,
                        Description = order.DesignDescription,
                        ImageUrl = ""  // Можна вказати посилання на зображення, якщо є
                    };
                }
                else if (order.DesignServices.Any())
                {
                    // Для замовлення з переліку беремо дані першої послуги
                    var ds = order.DesignServices.First();
                    portfolioItem = new PortfolioItem
                    {
                        Title = ds.Name,
                        Description = ds.Description,
                        ImageUrl = ""
                    };
                }
                if (portfolioItem != null)
                {
                    _context.PortfolioItems.Add(portfolioItem);
                }
                _context.SaveChanges();
            }
        }
    }
}

