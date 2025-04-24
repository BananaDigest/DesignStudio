using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DesignStudio.BLL.DTOs;
using DesignStudio.BLL.Interfaces;
using DesignStudio.DAL.Repositories;

namespace DesignStudio.BLL.Services
{
    public class PortfolioManager : IPortfolioManager
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public PortfolioManager(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PortfolioItemDto>> GetPortfolioAsync()
        {
            var items = await _uow.Portfolio.GetWithIncludeAsync(pi => pi.DesignService);
            return _mapper.Map<IEnumerable<PortfolioItemDto>>(items);
        }
    }
}
