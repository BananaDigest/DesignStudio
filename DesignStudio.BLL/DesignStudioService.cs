using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using DesignStudio.DAL.Data;
using DesignStudio.DAL.Models;
using DesignStudio.BLL.Interfaces;

namespace DesignStudio.BLL.Services
{
    public class DesignStudioService
    {
        private readonly DesignStudioContext _context;
        private readonly IOrderFactory _orderFactory;

        public DesignStudioService(DesignStudioContext context, IOrderFactory factory)
        {
            _context = context;
            _orderFactory = factory;
        }

        // === Послуги ===

        public void AddDesignService(DesignService service)
        {
            _context.DesignServices.Add(service);
            _context.SaveChanges();
        }

        public IEnumerable<DesignService> GetAllServices()
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

        public void DeleteDesignService(int id)
        {
            var s = _context.DesignServices.Find(id);
            if (s != null)
            {
                _context.DesignServices.Remove(s);
                _context.SaveChanges();
            }
        }

        public DesignService? GetServiceById(int id)
        {
            return _context.DesignServices
                .Include(s => s.Orders)
                .FirstOrDefault(s => s.DesignServiceId == id);
        }

        // === Замовлення ===

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

            if (order == null || order.Status == OrderStatus.Completed)
                return;

            order.Status = OrderStatus.Completed;
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

        // === Портфоліо ===

        public IEnumerable<PortfolioItem> GetPortfolio()
        {
            return _context.PortfolioItems
                .Include(p => p.DesignService)
                .ToList();
        }
    }
}
