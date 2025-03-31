using System.Collections.Generic;
using System.Linq;
using DesignStudio.DAL.Data;
using DesignStudio.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DesignStudio.BLL
{
    // Клас сервісу, який реалізує операції над сутностями
    public class DesignStudioService
    {
        private readonly DesignStudioContext _context;

        public DesignStudioService(DesignStudioContext context)
        {
            _context = context;
        }

        // CRUD-операції для DesignService

        // Створення нової послуги
        public void AddDesignService(DesignService service)
        {
            _context.DesignServices.Add(service);
            _context.SaveChanges();
        }

        public IEnumerable<DesignService> GetDesignServices()
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

        public void DeleteDesignService(int serviceId)
        {
            var service = _context.DesignServices.Find(serviceId);
            if (service != null)
            {
                _context.DesignServices.Remove(service);
                _context.SaveChanges();
            }
        }
    }
}

