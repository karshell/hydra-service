using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Hydra.WebApi.Configuration
{
    public class SendGridConfiguration
    {
        public string ApiKey { get; set; }

        public string FromEmail { get; set; }

        public string[] Recipients { get; set; }
    }
}
