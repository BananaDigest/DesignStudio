using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DesignStudio.BLL.DTOs;
using DesignStudio.BLL.Interfaces;
using DesignStudio.DAL.Models;
using DesignStudio.DAL.Repositories;

namespace DesignStudio.BLL.Services
{
    public class ServiceManager : IServiceManager
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public ServiceManager(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DesignServiceDto>> GetServicesAsync()
        {
            var list = await _uow.Services.GetAllAsync();
            return _mapper.Map<IEnumerable<DesignServiceDto>>(list);
        }

        public async Task AddServiceAsync(DesignServiceDto dto)
        {
            var entity = _mapper.Map<DesignService>(dto);
            await _uow.Services.AddAsync(entity);
            await _uow.CommitAsync();
        }

        public async Task UpdateServiceAsync(DesignServiceDto dto)
{
    // 1) Завантажуємо ту ж саму сутність з контексту (EF її трекатиме)
    var entity = await _uow.Services.GetByIdAsync(dto.Id);
    if (entity == null)
        return;

    // 2) Оновлюємо йому потрібні поля
    entity.Name = dto.Name;
    entity.Description = dto.Description;
    entity.Price = dto.Price;

    // 3) Комітимо зміни
    _uow.Services.Update(entity);
    await _uow.CommitAsync();
}


        public async Task DeleteServiceAsync(int id)
        {
            var entity = await _uow.Services.GetByIdAsync(id);
            if (entity != null)
            {
                _uow.Services.Remove(entity);
                await _uow.CommitAsync();
            }
        }
    }
}
