using Microsoft.AspNetCore.Mvc;
using DesignStudio.BLL.DTOs;
using DesignStudio.BLL.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;

namespace DesignStudio.API.Controllers
{

    [ApiController]
    [Route("api/designservices")]
    public class DesignServicesController : ControllerBase
    {
        private readonly IDesignStudioService _svc;
    private readonly IMapper _mapper;
    private readonly ILogger<DesignServicesController> _logger;

    public DesignServicesController(IDesignStudioService svc, IMapper mapper, ILogger<DesignServicesController> logger)
    {
        _svc = svc;
        _mapper = mapper;
        _logger = logger;
    }
     [HttpGet]
public async Task<IActionResult> GetAll()
{
    var bll = await _svc.GetServicesAsync();
    var apiDtos = _mapper.Map<IEnumerable<DesignServiceDto>>(bll);
    return Ok(apiDtos);
}


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var bllList = await _svc.GetServicesAsync();
            var item = bllList.FirstOrDefault(x => x.Id == id);
            if (item == null) return NotFound();
            return Ok(_mapper.Map<DesignServiceDto>(item));
        }

        [HttpPost]
    public async Task<IActionResult> Create(DesignServiceDto dto)
    {
        _logger.LogInformation(">>> Hit POST api/designservices");
        var bllDto = _mapper.Map<DesignStudio.BLL.DTOs.DesignServiceDto>(dto);
        await _svc.AddServiceAsync(bllDto);
        _logger.LogInformation(">>> Created service with ID {Id}", dto.Id);
        return CreatedAtAction(nameof(GetAll), new { id = dto.Id }, dto);
    }

        [HttpPut]
        public async Task<IActionResult> Update(DesignServiceDto dto)
        {
            var bllDto = _mapper.Map<DesignStudio.BLL.DTOs.DesignServiceDto>(dto);
            await _svc.UpdateServiceAsync(bllDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _svc.DeleteServiceAsync(id);
            return NoContent();
        }
    }
}
