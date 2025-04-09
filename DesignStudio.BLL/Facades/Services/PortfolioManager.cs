using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using DesignStudio.DAL.Data;
using DesignStudio.DAL.Models;

namespace DesignStudio.BLL.Services
{
    public class PortfolioManager
    {
        private readonly DesignStudioContext _context;
    
        public PortfolioManager(DesignStudioContext context)
        {
            _context = context;
        }
    
        public IEnumerable<PortfolioItem> GetPortfolio()
        {
            return _context.PortfolioItems
                .Include(p => p.DesignService)
                .ToList();
        }
    }
}
