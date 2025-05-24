using Microsoft.AspNetCore.Mvc;
using DesignStudio.BLL.DTOs;
using DesignStudio.BLL.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;

namespace DesignStudio.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PortfolioController : ControllerBase
    {
        private readonly IDesignStudioService _svc;
        private readonly IMapper _mapper;

        public PortfolioController(IDesignStudioService svc, IMapper mapper)
        {
            _svc = svc;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var bll = await _svc.GetPortfolioAsync();
            return Ok(_mapper.Map<IEnumerable<PortfolioItemDto>>(bll));
        }
    }
}
