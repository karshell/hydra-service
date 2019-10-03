using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Services.Hydra.WebApi.Configuration;
using Services.Hydra.WebApi.Models;
using Services.Hydra.WebApi.NotificationStrategies;

namespace Services.Hydra.WebApi.Services
{
    public class FillStateService : IFillStateService
    {
        private readonly NotificationSettings _notificationSettings;
        private readonly IDocumentStorageService _documentStorageService;
        private readonly IEnumerable<INotificationStrategy> _notificationStrategies;
        
        //necessary to account for sensor inconsistencies
        private const int NumConsecutiveReadings = 10;

        public FillStateService(IOptions<NotificationSettings> notificationSettings,
            IDocumentStorageService documentStorageService, IEnumerable<INotificationStrategy> notificationStrategies)
        {
            _notificationSettings = notificationSettings.Value;
            _documentStorageService = documentStorageService;
            _notificationStrategies = notificationStrategies;
        }

        public async Task ProcessFillState(ContainerFillState fillState)
        {
            await _documentStorageService.InsertAsync(fillState);

            //check if a notification has been sent recently
            NotificationHistory mostRecentNotification =
                (await _documentStorageService.GetManyAsync<NotificationHistory, DateTimeOffset>(
                    null, h => h.NotificationTime, 1)).FirstOrDefault();

            if (mostRecentNotification?.NotificationTime >=
                DateTimeOffset.Now.Subtract(TimeSpan.FromMinutes(_notificationSettings.MinutesBetweenNotifications)))
            {
                return;
            }

            //examine sensor history to confirm a stable state
            IEnumerable<ContainerFillState> mostRecentStates = await _documentStorageService.GetManyAsync<ContainerFillState, DateTimeOffset>(
                c => c.Container.ContainerId == fillState.Container.ContainerId,
                c => c.Timestamp.Value, NumConsecutiveReadings, false);

            if (mostRecentStates.All(s => !s.FillState))
            {
                foreach (INotificationStrategy notificationStrategy in _notificationStrategies)
                {
                    await notificationStrategy.Notify();
                }
            }
        }
    }
}
