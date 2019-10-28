using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using Services.Hydra.WebApi.Configuration;

namespace Services.Hydra.WebApi.NotificationStrategies
{
    public class EmailNotificationStrategy : INotificationStrategy
    {
        private readonly SendGridConfiguration _options;
        private readonly SendGridClient _client;

        public EmailNotificationStrategy(IOptions<SendGridConfiguration> options)
        {
            _options = options.Value;
            _client = new SendGridClient(_options.ApiKey);
        }

        public async Task Notify()
        {
            var from = new EmailAddress(_options.FromEmail);
            var subject = "Container Level Low";
            var content = "Low water level detected";

            foreach (var recipient in _options.Recipients)
            {
                var msg = MailHelper.CreateSingleEmail(from, new EmailAddress(recipient), subject, content, content);
                Response response = await _client.SendEmailAsync(msg);

                Console.WriteLine($"Notification email sent to {recipient} with result {response.StatusCode}");
            }
        }
    }
}
