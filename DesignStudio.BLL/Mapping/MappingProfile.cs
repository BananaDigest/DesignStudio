using AutoMapper;
using DesignStudio.BLL.DTOs;
using DesignStudio.DAL.Models;

namespace DesignStudio.BLL.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // → DesignService ↔ DesignServiceDto
            CreateMap<DesignService, DesignServiceDto>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.DesignServiceId));
            CreateMap<DesignServiceDto, DesignService>()
                .ForMember(e => e.DesignServiceId, o => o.MapFrom(d => d.Id));

            // → Order ↔ OrderDto
            CreateMap<Order, OrderDto>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.OrderId))
                .ForMember(d => d.Services, o => o.MapFrom(s => s.DesignServices));

            CreateMap<OrderDto, Order>()
                .ForMember(e => e.OrderId, o => o.MapFrom(d => d.Id))
                .ForMember(e => e.DesignServices, o => o.MapFrom(d => d.Services));

            // → PortfolioItem ↔ PortfolioItemDto
            CreateMap<PortfolioItem, PortfolioItemDto>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.PortfolioItemId))
                .ForMember(d => d.Service, o => o.MapFrom(s => s.DesignService));
            CreateMap<PortfolioItemDto, PortfolioItem>()
                .ForMember(e => e.PortfolioItemId, o => o.MapFrom(d => d.Id));
        }
    }
}
