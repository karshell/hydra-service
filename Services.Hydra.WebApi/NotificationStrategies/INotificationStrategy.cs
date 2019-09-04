using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Hydra.WebApi.NotificationStrategies
{
    public interface INotificationStrategy
    {
        Task Notify();
    }
}
