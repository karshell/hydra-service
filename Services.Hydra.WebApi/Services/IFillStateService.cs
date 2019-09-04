using System.Threading.Tasks;
using Services.Hydra.WebApi.Models;

namespace Services.Hydra.WebApi.Services
{
    public interface IFillStateService
    {
        Task ProcessFillState(ContainerFillState fillState);
    }
}
