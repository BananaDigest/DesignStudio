using System.Collections.Generic;
using System.Threading.Tasks;
using DesignStudio.BLL.DTOs;

namespace DesignStudio.BLL.Interfaces
{
    public interface IServiceManager
    {
        Task<IEnumerable<DesignServiceDto>> GetServicesAsync();
        Task AddServiceAsync(DesignServiceDto dto);
        Task UpdateServiceAsync(DesignServiceDto dto);
        Task DeleteServiceAsync(int id);
    }
}
