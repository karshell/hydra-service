using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Services.Hydra.DTO;
using Services.Hydra.WebApi.Models;

namespace Services.Hydra.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FillStateController : Controller
    {
        private readonly IMapper _mapper;

        public FillStateController(IMapper mapper)
        {
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> ReportFillState([FromBody] ContainerFillStateDTO state)
        {
            var container = _mapper.Map<ContainerFillState>(state);

            Console.Write(container);

            return Accepted();
        }
    }
}