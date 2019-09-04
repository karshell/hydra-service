using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Services.Hydra.DTO;
using Services.Hydra.WebApi.Models;
using Services.Hydra.WebApi.Services;

namespace Services.Hydra.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FillStateController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IFillStateService _fillStateService;

        public FillStateController(IMapper mapper, IFillStateService fillStateService)
        {
            _mapper = mapper;
            _fillStateService = fillStateService;
        }

        [HttpPost]
        public async Task<IActionResult> ReportFillState([FromBody] ContainerFillStateDTO state)
        {
            var container = _mapper.Map<ContainerFillState>(state);

            await _fillStateService.ProcessFillState(container);

            return Accepted();
        }
    }
}