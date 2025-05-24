using AutoMapper;
using DesignStudio.API.DTOs;
using DesignStudio.BLL.DTOs;

namespace DesignStudio.API.Mapping
{
    public class ApiMappingProfile : Profile
    {
        public ApiMappingProfile()
        {
            // API → BLL
            CreateMap<DesignServiceDtoApi, DesignServiceDto>();
            CreateMap<OrderDtoApi, OrderDto>();
            CreateMap<PortfolioItemDtoApi, PortfolioItemDto>();

            // Якщо потрібно, й інверсу
            CreateMap<DesignServiceDto, DesignServiceDtoApi>();
            CreateMap<OrderDto, OrderDtoApi>();
            CreateMap<PortfolioItemDto, PortfolioItemDtoApi>();
        }
    }
}
