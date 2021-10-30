using Microsoft.Extensions.Logging;
using VetClinicPublic.Interfaces;
using VetClinicPublic.Models;

namespace VetClinicPublic.Services
{
    public class LoggingMessagePublisher : IMessagePublisher
    {
        private readonly ILogger<IMessagePublisher> _logger;

        public LoggingMessagePublisher(ILogger<IMessagePublisher> logger)
        {
            _logger = logger;
        }

        public void Publish(AppointmentConfirmLinkClickedIntegrationEvent eventToPublish)
        {
            _logger.LogInformation("Published event {@Event}", eventToPublish);
        }
    }
}
