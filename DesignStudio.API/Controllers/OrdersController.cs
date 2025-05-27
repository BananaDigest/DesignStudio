using Microsoft.AspNetCore.Mvc;
using DesignStudio.BLL.DTOs;
using DesignStudio.BLL.Interfaces;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using System.Threading.Tasks;

namespace DesignStudio.API.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrdersController : ControllerBase
    {
        private readonly IDesignStudioService _svc;
        private readonly IMapper _mapper;

        public OrdersController(IDesignStudioService svc, IMapper mapper)
        {
            _svc = svc;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var bll = await _svc.GetOrdersAsync();
            return Ok(_mapper.Map<IEnumerable<OrderDto>>(bll));
        }

        [HttpPost("turnkey")]
        public async Task<IActionResult> CreateTurnkey(OrderDto dto)
        {
            var bllDto = _mapper.Map<DesignStudio.BLL.DTOs.OrderDto>(dto);
            await _svc.CreateTurnkeyOrderAsync(bllDto);
            return CreatedAtAction(nameof(GetAll), null);
        }

        [HttpPost("service")]
        public async Task<IActionResult> CreateServiceOrder(OrderDto dto)
        {
            var bllDto = _mapper.Map<DesignStudio.BLL.DTOs.OrderDto>(dto);
            await _svc.CreateServiceOrderAsync(bllDto);
            return CreatedAtAction(nameof(GetAll), null);
        }

        [HttpPut("{id}/complete")]
        public async Task<IActionResult> MarkCompleted(int id)
        {
            await _svc.MarkOrderCompletedAsync(id);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _svc.DeleteOrderAsync(id);
            return NoContent();
        }
    }
}
