using AutoMapper;
using DesignStudio.BLL.DTOs;
using DesignStudio.BLL.Interfaces;
using DesignStudio.DAL.Models;
using DesignStudio.DAL.Repositories;

namespace DesignStudio.BLL.Services
{
    public class OrderManager : IOrderManager
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public OrderManager(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OrderDto>> GetOrdersAsync()
        {
            var orders = await _uow.Orders.GetWithIncludeAsync(o => o.DesignServices);
            return _mapper.Map<IEnumerable<OrderDto>>(orders);
        }

        public async Task CreateTurnkeyOrderAsync(OrderDto dto)
        {
            await _uow.BeginTransactionAsync();
            try
            {
                var order = _mapper.Map<Order>(dto);
                await _uow.Orders.AddAsync(order);
                await _uow.CommitTransactionAsync();
            }
            catch
            {
                await _uow.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task CreateServiceOrderAsync(OrderDto dto)
        {
            await _uow.BeginTransactionAsync();
            try
            {
                var order = _mapper.Map<Order>(dto);
                await _uow.Orders.AddAsync(order);
                await _uow.CommitTransactionAsync();
            }
            catch
            {
                await _uow.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task DeleteOrderAsync(int id)
        {
            var order = await _uow.Orders.GetByIdAsync(id);
            if (order != null)
            {
                _uow.Orders.Remove(order);
                await _uow.CommitAsync();
            }
        }

        public async Task MarkOrderCompletedAsync(int orderId)
        {
            await _uow.BeginTransactionAsync();
            try
            {
                var order = (await _uow.Orders
                    .GetWithIncludeAsync(o => o.DesignServices))
                    .FirstOrDefault(o => o.OrderId == orderId);

                if (order == null)
                {
                    await _uow.RollbackTransactionAsync();
                    return;
                }

                _uow.Orders.Remove(order);

                PortfolioItem item;
                if (order.IsTurnkey)
                {
                    item = new PortfolioItem
                    {
                        Title = order.DesignRequirement ?? "Проєкт",
                        Description = order.DesignDescription ?? "Опис відсутній"
                    };
                }
                else
                {
                    var svc = order.DesignServices.First();
                    item = new PortfolioItem
                    {
                        Title = svc.Name,
                        Description = svc.Description,
                        DesignService = svc
                    };
                }

                await _uow.Portfolio.AddAsync(item);

                await _uow.CommitTransactionAsync();
            }
            catch
            {
                await _uow.RollbackTransactionAsync();
                throw;
            }
        }
    }
}
