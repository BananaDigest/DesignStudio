using System.Collections.Generic;
using System.Threading.Tasks;
using DesignStudio.BLL.DTOs;

namespace DesignStudio.BLL.Interfaces
{
    public interface IPortfolioManager
    {
        Task<IEnumerable<PortfolioItemDto>> GetPortfolioAsync();
    }
}
