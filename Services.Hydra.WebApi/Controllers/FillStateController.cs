using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services.Hydra.DTO;

namespace Services.Hydra.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FillStateController : Controller
    {
        [HttpPost]
        public async Task ReportFillState([FromBody] FillStateDTO state)
        {
        }
    }
}