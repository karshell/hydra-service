using System;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Services.Hydra.WebApi.Configuration;

namespace Services.Hydra.WebApi.NotificationStrategies
{
    public class GoogleNotificationStrategy : INotificationStrategy
    {
        private readonly AssistantRelaySettings _settings;
        private readonly IHttpClientFactory _httpClientFactory;

        public GoogleNotificationStrategy(IOptions<AssistantRelaySettings> settings, IHttpClientFactory httpClientFactory)
        {
            _settings = settings.Value;
            _httpClientFactory = httpClientFactory;
        }

        public async Task Notify()
        {
            HttpClient client = _httpClientFactory.CreateClient();

            AssistantRelayRequest request = new AssistantRelayRequest
            {
                User = "zoey",
                Command = "water dish is empty",
                Broadcast = true
            };
            
            HttpResponseMessage response = await client.PostAsync(_settings.AssistantRelayUri,
                new StringContent(JsonConvert.SerializeObject(request)));

            Console.WriteLine($"Alert sent to assistant with response coee {response.StatusCode}");
        }
    }
}
