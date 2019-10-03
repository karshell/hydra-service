using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Services.Hydra.WebApi.Configuration;

namespace Services.Hydra.WebApi.NotificationStrategies
{
    public class GoogleNotificationStrategy : INotificationStrategy
    {
        private readonly AssistantRelaySettings _settings;

        public GoogleNotificationStrategy(IOptions<AssistantRelaySettings> settings)
        {
            _settings = settings.Value;
        }

        public Task Notify()
        {
            throw new NotImplementedException();
        }
    }
}
