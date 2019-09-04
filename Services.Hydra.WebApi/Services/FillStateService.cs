using System.Threading.Tasks;
using Services.Hydra.WebApi.Models;
using Services.Hydra.WebApi.NotificationStrategies;

namespace Services.Hydra.WebApi.Services
{
    public class FillStateService : IFillStateService
    {
        private readonly IDocumentStorageService _documentStorageService;
        private readonly INotificationStrategy[] _notificationStrategies;

        public FillStateService(IDocumentStorageService documentStorageService, INotificationStrategy[] notificationStrategies)
        {
            _documentStorageService = documentStorageService;
            _notificationStrategies = notificationStrategies;
        }

        public async Task ProcessFillState(ContainerFillState fillState)
        {
            //TODO: retrieve last x entries and check for consistency
            await _documentStorageService.InsertAsync(fillState);
        }
    }
}
