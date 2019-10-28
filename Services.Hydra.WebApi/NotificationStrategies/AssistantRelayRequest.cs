using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Hydra.WebApi.NotificationStrategies
{
    public class AssistantRelayRequest
    {
        public string Command { get; set; }

        public string User { get; set; }

        public bool Broadcast { get; set; }
    }
}
