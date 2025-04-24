using System;
using DesignStudio.DAL.Models;
using DesignStudio.BLL.Interfaces;

namespace DesignStudio.BLL
{
    public class OrderFactory : IOrderFactory
    {
        public Order CreateTurnkeyOrder(string customer, string phone, string requirement, string description)
        {
            return new Order
            {
                CustomerName = customer,
                Phone = phone,
                OrderDate = DateTime.Now,
                IsTurnkey = true,
                DesignRequirement = requirement,
                DesignDescription = description
            };
        }

        public Order CreateServiceOrder(string customer, string phone, DesignService service)
        {
            var order = new Order
            {
                CustomerName = customer,
                Phone = phone,
                OrderDate = DateTime.Now,
                IsTurnkey = false
            };
            order.DesignServices.Add(service);
            return order;
        }
    }
}
