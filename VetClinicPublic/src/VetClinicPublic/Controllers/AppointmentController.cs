using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VetClinicPublic.Interfaces;
using VetClinicPublic.Models;

namespace VetClinicPublic.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly ILogger<AppointmentController> _logger;
        private readonly IMessagePublisher _messagePublisher;

        public AppointmentController(ILogger<AppointmentController> logger, IMessagePublisher messagePublisher)
        {
            _logger = logger;
            _messagePublisher = messagePublisher;
        }

        public ActionResult Confirm(Guid id)
        {
            _logger.LogInformation("Hit the Appointment/Confirm route with {AppointmentId}", id);

            var @event = new AppointmentConfirmLinkClickedIntegrationEvent(id);
            _messagePublisher.Publish(@event);

            return View();
        }
    }
}
