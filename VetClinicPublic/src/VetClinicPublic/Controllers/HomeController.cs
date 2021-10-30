using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NArdalis.GuardClauses;
using VetClinicPublic.Configuration;
using VetClinicPublic.Interfaces;
using VetClinicPublic.Models;

namespace VetClinicPublic.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SiteConfiguration _siteConfiguration;
        private readonly ISendEmail _mailSender;

        private readonly ISendConfirmationEmail _confirmationEmailSender;

        public HomeController(ILogger<HomeController> logger,
                              IOptions<SiteConfiguration> siteConfiguration,
                              ISendEmail mailSender,
                              ISendConfirmationEmail confirmationEmailSender)
        {
            _logger = logger;
            _siteConfiguration = Guard.Against.Null(siteConfiguration?.Value, nameof(siteConfiguration));
            _mailSender = mailSender;
            _confirmationEmailSender = confirmationEmailSender;
        }

        public IActionResult Index()
        {
            _logger.LogInformation("Hit the Home/Index route");

            ViewBag.PapercutManagementPort = _siteConfiguration.PapercutManagementPort;

            return View();
        }

        public ActionResult TestEmail()
        {
            _logger.LogInformation("Hit the Home/TestEmail route");

            _mailSender.SendEmail(
                to: "test@test.com",
                from: "donotreply@test.com",
                subject: "Test",
                body: "This is just a test.");

            return Ok("Test email has been sent.");
        }

        public ActionResult TestConfirmationEmail(Guid id)
        {
            _logger.LogInformation("Hit the Home/TestConfirmationEmail route with {Id}", id);

            var testAppointment = new SendAppointmentConfirmationCommand
            {
                AppointmentId = id,
                ClientEmailAddress = "client@test.com",
                ClientName = "Mr. Test",
                PatientName = "Testy",
                AppointmentType = "Test",
                DoctorName = "Dr. Test",
                AppointmentStart = DateTime.Today
            };

            _confirmationEmailSender.SendConfirmationEmail(testAppointment);

            return Ok("Test confirmation email has been sent.");
        }
    }
}
