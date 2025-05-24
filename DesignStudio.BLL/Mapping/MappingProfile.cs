using AutoMapper;
using BllDtos = DesignStudio.BLL.DTOs;
using DesignStudio.DAL.Models;

namespace DesignStudio.BLL.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // ── 1) Мапінги між сутностями бази (DAL) та DTO бізнес-логіки (BLL) ────────────────────────────
            CreateMap<DesignService, BllDtos.DesignServiceDto>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.DesignServiceId));
            CreateMap<BllDtos.DesignServiceDto,  DesignService>()
                .ForMember(e => e.DesignServiceId, o => o.MapFrom(d => d.Id));

            CreateMap<Order, BllDtos.OrderDto>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.OrderId))
                .ForMember(d => d.Services, o => o.MapFrom(s => s.DesignServices));
            CreateMap<BllDtos.OrderDto,  Order>()
                .ForMember(e => e.OrderId, o => o.MapFrom(d => d.Id))
                .ForMember(e => e.DesignServices, o => o.MapFrom(d => d.Services));

            CreateMap<PortfolioItem, BllDtos.PortfolioItemDto>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.PortfolioItemId))
                .ForMember(d => d.Service, o => o.MapFrom(s => s.DesignService));
            CreateMap<BllDtos.PortfolioItemDto, PortfolioItem>()
                .ForMember(e => e.PortfolioItemId, o => o.MapFrom(d => d.Id))
                .ForMember(e => e.DesignService, o => o.MapFrom(d => d.Service));
        }
    }
}
