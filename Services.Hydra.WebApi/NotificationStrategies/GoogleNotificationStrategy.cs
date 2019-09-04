using System;
using System.Threading.Tasks;
using Services.Hydra.WebApi.Configuration;

namespace Services.Hydra.WebApi.NotificationStrategies
{
    public class GoogleNotificationStrategy : INotificationStrategy
    {
        private readonly AssistantRelaySettings _settings;

        public GoogleNotificationStrategy(AssistantRelaySettings settings)
        {
            _settings = settings;
        }

        public Task Notify()
        {
            throw new NotImplementedException();
        }
    }
}
