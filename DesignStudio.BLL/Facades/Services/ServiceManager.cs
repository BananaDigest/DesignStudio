using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using DesignStudio.DAL.Data;
using DesignStudio.DAL.Models;

namespace DesignStudio.BLL.Services
{
    public class ServiceManager
    {
        private readonly DesignStudioContext _context;
    
        public ServiceManager(DesignStudioContext context)
        {
            _context = context;
        }
    
        public void AddDesignService(DesignService service)
        {
            _context.DesignServices.Add(service);
            _context.SaveChanges();
        }
    
        public IEnumerable<DesignService> GetAllServices()
        {
            return _context.DesignServices
                .Include(ds => ds.PortfolioItems)
                .Include(ds => ds.Orders)
                .ToList();
        }
    
        public void UpdateDesignService(DesignService service)
        {
            _context.DesignServices.Update(service);
            _context.SaveChanges();
        }
    
        public void DeleteDesignService(int id)
        {
            var s = _context.DesignServices.Find(id);
            if (s != null)
            {
                _context.DesignServices.Remove(s);
                _context.SaveChanges();
            }
        }
    
        public DesignService? GetServiceById(int id)
        {
            return _context.DesignServices
                .Include(s => s.Orders)
                .FirstOrDefault(s => s.DesignServiceId == id);
        }
    }
}
